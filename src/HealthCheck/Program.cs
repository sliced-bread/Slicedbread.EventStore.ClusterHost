using System;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using HealthCheck.Models;
using Topshelf;

namespace HealthCheck
{
    internal class Program
    {
        private const string ClusterHostConfigFile = "Slicedbread.EventStore.ClusterHost.exe.config";
        
        static int Main(string[] args)
        {
            var config = LoadConfig(args);

            if (config == null)
            {
                return -1;
            }

            HostFactory.Run(
                hc =>
                {
                    hc.RunAsLocalSystem();
                    hc.StartAutomatically();
                    hc.EnableShutdown();
                    hc.EnableServiceRecovery(c => c.RestartService(1));

                    hc.Service<MonitorService>(
                        s =>
                        {
                            s.ConstructUsing(name => new MonitorService(config));
                            s.WhenStarted(tc => tc.Start());
                            s.WhenStopped(tc => tc.Stop());
                        });

                    hc.SetDescription("EventStore Node Probe Proxy");
                    hc.SetDisplayName("EventStore HealthCheck");
                    hc.SetServiceName("EventStoreHealthCheck");
                });

            Console.ReadLine();
            
            return 0;
        }

        private static HeathCheckConfiguration LoadConfig(string[] args)
        {
            var config = new HeathCheckConfiguration();

            config.ListenAddress = ConfigurationManager.AppSettings["ListenAddress"] ?? "http://localhost:6842/";

            var configPath = GetConfigPath();
            
            var serializer = new XmlSerializer(typeof(ClusterHostConfig));
            using (var reader = new StreamReader(configPath))
            {
                var clusterHostConfig = (ClusterHostConfig) serializer.Deserialize(reader);
                var eventStoreConfig = clusterHostConfig.EventStore;

                if (eventStoreConfig == null)
                {
                    Console.WriteLine("Missing eventStore configuration section");
                    return null;
                }

                if (eventStoreConfig.InternalNodes.Nodes.Count == 0)
                {
                    Console.WriteLine("No internal nodes configured");
                    return null;
                }

                if (eventStoreConfig.InternalNodes.Nodes.Count > 1)
                {
                    Console.WriteLine("Only a single node is supported");
                    return null;
                }

                config.EventStoreNode = eventStoreConfig.InternalNodes.Nodes[0];

                return config;
            }
        }

        private static string GetConfigPath()
        {
            Console.WriteLine("Checking for Slicedbread.EventStore.ClusterHost.exe.config..");

            var paths = new[] {"./", "../"};

            foreach (var testPath in paths)
            {
                var configPath = Path.GetFullPath(Path.Combine(testPath, ClusterHostConfigFile));
                Console.WriteLine($"Checking {configPath}");

                if (File.Exists(configPath))
                {
                    Console.WriteLine("Config file found");
                    return configPath;
                }
            }

            throw new InvalidOperationException("Unable to find Slicedbread.EventStore.ClusterHost.exe.config file");
        }
    }
}
