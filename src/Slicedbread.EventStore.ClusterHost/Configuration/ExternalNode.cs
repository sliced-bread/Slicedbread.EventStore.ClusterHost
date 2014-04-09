namespace Slicedbread.EventStore.ClusterHost.Configuration
{
    using System.Configuration;

    public class ExternalNode : Node
    {
        [ConfigurationProperty("ipAddress", IsRequired = true)]
        public string DbPath
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
    }
}