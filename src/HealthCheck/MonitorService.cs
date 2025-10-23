using System;
using HealthCheck.Models;
using Nancy;
using Nancy.Hosting.Self;

namespace HealthCheck
{
    public class MonitorService
    {
        // Icky static to make it accessible from the Nancy module
        // don't really need a container as it can only be run once
        public static HeathCheckConfiguration HeathCheckConfiguration;
        private NancyHost _host;

        public MonitorService(HeathCheckConfiguration heathCheckConfiguration)
        {
            HeathCheckConfiguration = heathCheckConfiguration;
        }

        public void Start()
        {
            Console.WriteLine($"Starting listening on {HeathCheckConfiguration.ListenAddress}");
            var url = new Uri(HeathCheckConfiguration.ListenAddress, UriKind.Absolute);
            var hostConfigs = new HostConfiguration
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true }
            };
            this._host = new NancyHost(hostConfigs, url);
            this._host.Start();
            Console.WriteLine("Listener started");
        }

        public void Stop()
        {
            Console.WriteLine("Stopping");
            if (this._host == null)
            {
                return;
            }

            try
            {
                this._host.Stop();
                this._host.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}