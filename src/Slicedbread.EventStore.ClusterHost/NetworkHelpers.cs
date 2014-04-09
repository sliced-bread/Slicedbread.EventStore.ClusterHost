namespace Slicedbread.EventStore.ClusterHost
{
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    public static class NetworkHelpers
    {
        public static IPAddress GetIPAddress()
        {
            var hostName = Dns.GetHostName();

            return Dns.GetHostAddresses(hostName).First(
                address =>
                    {
                        if (address.AddressFamily != AddressFamily.InterNetwork)
                        {
                            return false;
                        }

                        return !Equals(address, IPAddress.Loopback);
                    });
        }
    }
}