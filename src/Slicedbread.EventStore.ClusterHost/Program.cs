namespace Slicedbread.EventStore.ClusterHost
{
    using System;
    using System.Configuration;
    using System.IO;

    using Slicedbread.EventStore.ClusterHost.Configuration;
    using Slicedbread.EventStore.ClusterHost.Logging;

    using Topshelf;

    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = (EventStoreServiceConfiguration)ConfigurationManager.GetSection("eventStore");
            var address = NetworkHelpers.GetIPAddress();

            if (args.Length == 1 && args[0] == "--test")
            {
                var service = new EventStoreService(address, configuration, new ConsoleLogger());

                service.DumpConfig();

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
                                    s.ConstructUsing(name => new EventStoreService(address, configuration, new FileLogger(GetLogFilename())));
                                    s.WhenStarted(tc => tc.Start());
                                    s.WhenStopped(tc => tc.Stop());
                                });

                        hc.SetDescription("EventEngine EventStore Service");
                        hc.SetDisplayName("EventEngine EventStore");
                        hc.SetServiceName("EventEngineEventStore");
                    });

            Console.ReadLine();
        }

        private static string GetLogFilename()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "host.log");
        }
    }
}