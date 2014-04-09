namespace Slicedbread.EventStore.ClusterHost.Configuration
{
    using System.Configuration;

    public class ExternalNode : Node
    {
        [ConfigurationProperty("ipAddress", IsRequired = true)]
        public string IpAddress
        {
            get
            {
                return (string)this["ipAddress"];
            }
            set
            {
                this["ipAddress"] = value;
            }
        }

        [ConfigurationProperty("gossipPort", IsRequired = true)]
        public string GossipPort
        {
            get
            {
                return (string)this["gossipPort"];
            }
            set
            {
                this["gossipPort"] = value;
            }
        }
    }
}