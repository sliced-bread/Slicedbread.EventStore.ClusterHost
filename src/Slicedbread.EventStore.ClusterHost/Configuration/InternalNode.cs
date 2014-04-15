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

        [ConfigurationProperty("internalIpAddress", IsRequired = false)]
        public string InternalIpAddress
        {
            get
            {
                return (string)this["internalIpAddress"];
            }
            set
            {
                this["internalIpAddress"] = value;
            }
        }

        [ConfigurationProperty("externalIpAddress", IsRequired = false)]
        public string ExternalIpAddress
        {
            get
            {
                return (string)this["externalIpAddress"];
            }
            set
            {
                this["externalIpAddress"] = value;
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

        [ConfigurationProperty("extHttpPort", IsRequired = true)]
        public int ExtHttpPort
        {
            get
            {
                return (int)this["extHttpPort"];
            }
            set
            {
                this["extHttpPort"] = value;
            }
        }

        [ConfigurationProperty("extTcpPort", IsRequired = true)]
        public int ExtTcpPort
        {
            get
            {
                return (int)this["extTcpPort"];
            }
            set
            {
                this["extTcpPort"] = value;
            }
        }

        [ConfigurationProperty("intHttpPort", IsRequired = true)]
        public int IntHttpPort
        {
            get
            {
                return (int)this["intHttpPort"];
            }
            set
            {
                this["intHttpPort"] = value;
            }
        }

        [ConfigurationProperty("intTcpPort", IsRequired = true)]
        public int IntTcpPort
        {
            get
            {
                return (int)this["intTcpPort"];
            }
            set
            {
                this["intTcpPort"] = value;
            }
        }

        [ConfigurationProperty("additionalFlags", IsRequired = false)]
        public string AdditionalFlags
        {
            get
            {
                return (string)this["additionalFlags"];
            }

            set
            {
                this["additionalFlags"] = value;
            }
        }
    }
}