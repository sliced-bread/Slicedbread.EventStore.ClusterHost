namespace Slicedbread.EventStore.ClusterHost.Configuration
{
    using System.Configuration;

    public class Node : ConfigurationElement
    {
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