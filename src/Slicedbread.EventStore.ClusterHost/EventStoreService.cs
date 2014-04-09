namespace Slicedbread.EventStore.ClusterHost
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;

    using Slicedbread.EventStore.ClusterHost.Configuration;
    using Slicedbread.EventStore.ClusterHost.Logging;

    public class EventStoreService
    {
        private const string ExecutableName = "EventStore.ClusterNode.exe";

        private readonly IPAddress address;

        private readonly EventStoreServiceConfiguration configuration;

        private readonly string eventStorePath;

        private readonly ILogger logger;

        private readonly IDictionary<Process, Tuple<string, DateTime>> processes;

        private readonly int restartDelay;

        private readonly object restartPadlock = new object();

        private readonly TimeSpan restartWindow;

        private bool stopping;

        public EventStoreService(IPAddress address, EventStoreServiceConfiguration configuration, ILogger logger)
        {
            this.address = address;
            this.configuration = configuration;
            this.logger = logger;
            this.processes = new Dictionary<Process, Tuple<string, DateTime>>();
            this.eventStorePath = this.GetEventStorePath(configuration);

            this.restartDelay = this.GetRestartDelay(configuration);
            this.restartWindow = this.GetRestartWindow(configuration);

            this.DumpConfig();
        }

        public void DumpConfig()
        {
            var nodeCount = this.configuration.InternalNodes.Count;

            this.logger.Log("Configuration Settings");
            this.logger.Log("++++++++++++++++++++++\n");
            this.logger.Log(string.Format("Nodes Configured: {0}", nodeCount));
            this.logger.Log(string.Format("EventStore Path: {0}", this.eventStorePath));
            this.logger.Log(string.Format("Restart Delay (ms): {0}", this.restartDelay));
            this.logger.Log(string.Format("Restart Window (ms): {0}", this.restartWindow.TotalMilliseconds));
            this.logger.Log("Node Configurations:");

            foreach (InternalNode node in this.configuration.InternalNodes)
            {
                var arguments = this.BuildNodeArguments(node, this.configuration, nodeCount, this.address);
                this.logger.Log(string.Format("\t{0} : {1}", node.Name, arguments));
            }
        }

        public void Start()
        {
            this.logger.Log("Starting");

            var nodeCount = this.configuration.InternalNodes.Count;

            this.logger.Log(string.Format("Starting {0} nodes", nodeCount));

            foreach (InternalNode node in this.configuration.InternalNodes)
            {
                var arguments = this.BuildNodeArguments(node, this.configuration, nodeCount, this.address);

                var processStartInfo = new ProcessStartInfo(this.eventStorePath, arguments) { UseShellExecute = false };

                var process = Process.Start(processStartInfo);

                if (process == null)
                {
                    this.Stop();

                    this.logger.Log("Unable to launch processes");

                    throw new InvalidOperationException("Unable to start all processes");
                }

                process.EnableRaisingEvents = true;

                this.processes.Add(process, new Tuple<string, DateTime>(node.Name, DateTime.Now));

                process.Exited += this.AttemptProcessRestart;
            }
        }

        public void Stop()
        {
            this.logger.Log("Stopping");

            this.stopping = true;

            foreach (var kvp in this.processes)
            {
                var nodeName = kvp.Value.Item1;

                var process = kvp.Key;

                process.Refresh();

                if (!process.HasExited)
                {
                    try
                    {
                        this.logger.Log(string.Format("Killing: {0}", nodeName));

                        process.Kill();
                        process.WaitForExit();
                        process.Dispose();

                        this.logger.Log("Process killed");
                    }
                        // ReSharper disable once EmptyGeneralCatchClause
                    catch
                    {
                        this.logger.Log("Kill failed!");
                    }
                }
                else
                {
                    this.logger.Log(string.Format("Process already exited: {0}", nodeName));
                }
            }
        }

        private void AttemptProcessRestart(object sender, EventArgs e)
        {
            if (this.stopping)
            {
                this.logger.Log("Service stopping so auto-restart disabled");
                return;
            }

            var process = (Process)sender;

            lock (this.restartPadlock)
            {
                Tuple<string, DateTime> processEntry;

                if (!this.processes.TryGetValue(process, out processEntry))
                {
                    return;
                }

                var nodeName = processEntry.Item1;
                var startedTime = processEntry.Item2;

                this.logger.Log(string.Format("Process '{0}' has terminated.", nodeName));

                var currentTime = DateTime.Now;
                var windowEnd = startedTime.Add(this.restartWindow);

                if (windowEnd > currentTime)
                {
                    this.logger.Log(
                        string.Format(
                            "Still inside restart window, not restarting. (Started at: {0} Now: {1} Window End: {2}",
                            startedTime,
                            currentTime,
                            windowEnd));
                    return;
                }

                this.logger.Log(string.Format("Process needs to be restarted - sleeping for {0}s", this.restartDelay / 1000));

                Thread.Sleep(this.restartDelay);

                this.logger.Log("Starting process");

                var startResult = process.Start();

                this.logger.Log(startResult ? "Started sucessfully, updating last start time" : "Start failed!");

                this.processes[process] = new Tuple<string, DateTime>(nodeName, DateTime.Now);
            }
        }

        private string BuildNodeArguments(
            InternalNode currentNode,
            EventStoreServiceConfiguration configuration,
            int nodeCount,
            IPAddress ipAddress)
        {
            // Based on https://github.com/eventstore/eventstore/wiki/Setting-Up-OSS-Cluster
            var builder = new StringBuilder();

            builder.AppendFormat("--db {0} ", currentNode.DbPath);
            builder.AppendFormat("--log {0} ", currentNode.LogPath);
            builder.AppendFormat("--int-ip {0} ", ipAddress);
            builder.AppendFormat("--ext-ip {0} ", ipAddress);

            if (!string.IsNullOrWhiteSpace(currentNode.HttpPrefix))
            {
                builder.AppendFormat("--http-prefix={0} ", currentNode.HttpPrefix);
            }

            builder.AppendFormat("--int-tcp-port {0} ", currentNode.IntTcpPort);
            builder.AppendFormat("--ext-tcp-port {0} ", currentNode.ExtTcpPort);
            builder.AppendFormat("--int-http-port {0} ", currentNode.IntHttpPort);
            builder.AppendFormat("--ext-http-port {0} ", currentNode.ExtHttpPort);

            builder.AppendFormat("--nodes-count {0} ", nodeCount);

            builder.AppendFormat("--use-dns-discovery- ");

            foreach (var otherNode in configuration.InternalNodes.Cast<InternalNode>().Where(n => !ReferenceEquals(n, currentNode)))
            {
                builder.AppendFormat("--gossip-seed {0}:{1} ", ipAddress, otherNode.IntHttpPort);
            }

            return builder.ToString();
        }

        private string GetEventStorePath(EventStoreServiceConfiguration config)
        {
            var configPath = config.EventStorePath;

            if (string.IsNullOrWhiteSpace(configPath))
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "eventstore", ExecutableName);
            }

            if (Path.IsPathRooted(configPath))
            {
                return configPath;
            }

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configPath, ExecutableName);
        }

        private int GetRestartDelay(EventStoreServiceConfiguration config)
        {
            return config.RestartDelayMs.HasValue ? config.RestartDelayMs.Value : 1000 * 3;
        }

        private TimeSpan GetRestartWindow(EventStoreServiceConfiguration config)
        {
            var windowInMs = config.RestartWindowMs.HasValue ? config.RestartWindowMs.Value : 1000 * 30;

            return new TimeSpan(0, 0, 0, 0, windowInMs);
        }
    }
}