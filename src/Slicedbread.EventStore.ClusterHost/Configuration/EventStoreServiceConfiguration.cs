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

        [ConfigurationProperty("nodes", IsDefaultCollection = false, IsKey = false, IsRequired = true)]
        public NodeCollection Nodes
        {
            get
            {
                return (NodeCollection)this["nodes"];
            }

            set
            {
                this["nodes"] = value;
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