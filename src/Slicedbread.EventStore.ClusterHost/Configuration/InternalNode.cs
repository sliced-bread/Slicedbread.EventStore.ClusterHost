namespace Slicedbread.EventStore.ClusterHost.Configuration
{
    using System.Configuration;

    public class InternalNode : Node
    {
        [ConfigurationProperty("dbPath", IsRequired = true)]
        public string DbPath
        {
            get
            {
                return (string)this["dbPath"];
            }
            set
            {
                this["dbPath"] = value;
            }
        }

        [ConfigurationProperty("httpPrefix", IsRequired = false)]
        public string HttpPrefix
        {
            get
            {
                return (string)this["httpPrefix"];
            }

            set
            {
                this["httpPrefix"] = value;
            }
        }

        [ConfigurationProperty("logPath", IsRequired = true)]
        public string LogPath
        {
            get
            {
                return (string)this["logPath"];
            }
            set
            {
                this["logPath"] = value;
            }
        }
    }
}