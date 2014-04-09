namespace Slicedbread.EventStore.ClusterHost.Configuration
{
    using System.Configuration;

    public class EventStoreServiceConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("eventStorePath", IsRequired = false)]
        public string EventStorePath
        {
            get
            {
                return (string)this["eventStorePath"];
            }

            set
            {
                this["eventStorePath"] = value;
            }
        }

        [ConfigurationProperty("internalNodes", IsDefaultCollection = false, IsKey = false, IsRequired = true)]
        public NodeCollection InternalNodes
        {
            get
            {
                return (NodeCollection)this["internalNodes"];
            }

            set
            {
                this["internalNodes"] = value;
            }
        }

        [ConfigurationProperty("externalNodes", IsDefaultCollection = false, IsKey = false, IsRequired = true)]
        public ExternalNodeCollection ExternalNodes
        {
            get
            {
                return (ExternalNodeCollection)this["externalNodes"];
            }

            set
            {
                this["externalNodes"] = value;
            }
        }

        [ConfigurationProperty("restartDelayMs", IsRequired = false)]
        public int? RestartDelayMs
        {
            get
            {
                return (int?)this["restartDelayMs"];
            }

            set
            {
                this["restartDelayMs"] = value;
            }
        }

        [ConfigurationProperty("restartWindowMs", IsRequired = false)]
        public int? RestartWindowMs
        {
            get
            {
                return (int?)this["restartWindowMs"];
            }

            set
            {
                this["restartWindowMs"] = value;
            }
        }
    }
}