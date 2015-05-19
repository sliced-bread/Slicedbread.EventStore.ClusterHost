using System.Linq;

namespace Slicedbread.EventStore.ClusterHost
{
    using System;
    using System.Configuration;

    using NLog;
    using NLog.Config;
    using NLog.Targets;

    using Slicedbread.EventStore.ClusterHost.Configuration;

    using Topshelf;

    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = (EventStoreServiceConfiguration)ConfigurationManager.GetSection("eventStore");
            var address = NetworkHelpers.GetIPAddress();

            if (args.Contains("--console", StringComparer.CurrentCultureIgnoreCase))
            {
                var config = new LoggingConfiguration();
                var consoleTarget = new ColoredConsoleTarget { Layout = "${message}" };
                config.AddTarget("console", consoleTarget);
                var rule = new LoggingRule("*", LogLevel.Debug, consoleTarget);
                config.LoggingRules.Add(rule);

                LogManager.Configuration = config;
            }

            if (args.Length == 1 && args[0] == "--test")
            {
                var config = new LoggingConfiguration();
                var consoleTarget = new ColoredConsoleTarget { Layout = "${message}" };
                config.AddTarget("console", consoleTarget);
                var rule = new LoggingRule("*", LogLevel.Debug, consoleTarget);
                config.LoggingRules.Add(rule);

                LogManager.Configuration = config;

                var logger = LogManager.GetLogger("config");

                // ReSharper disable once ObjectCreationAsStatement
                new EventStoreService(address, configuration, logger);

                return;
            }

            HostFactory.Run(
                hc =>
                    {
                        hc.RunAsLocalSystem();
                        hc.StartAutomatically();
                        hc.EnableShutdown();
                        hc.EnableServiceRecovery(c => c.RestartService(1));

                        hc.Service<EventStoreService>(
                            s =>
                                {
                                    s.ConstructUsing(name => new EventStoreService(address, configuration, LogManager.GetLogger("eventstorehost")));
                                    s.WhenStarted(tc => tc.Start());
                                    s.WhenStopped(tc => tc.Stop());
                                });

                        hc.SetDescription("EventStore OSS Cluster Host Service");
                        hc.SetDisplayName("EventStore Host");
                        hc.SetServiceName("EventStoreHost");
                    });

            Console.ReadLine();
        }
    }
}