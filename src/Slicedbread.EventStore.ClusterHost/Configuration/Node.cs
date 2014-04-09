namespace Slicedbread.EventStore.ClusterHost.Configuration
{
    using System.Configuration;

    public class Node : ConfigurationElement
    {
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

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
    }
}