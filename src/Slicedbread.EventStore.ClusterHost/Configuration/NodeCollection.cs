namespace Slicedbread.EventStore.ClusterHost.Configuration
{
    using System;
    using System.Configuration;

    public class NodeCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return "node";
            }
        }

        public Node this[int index]
        {
            get
            {
                return this.BaseGet(index) as Node;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Node();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Node)element).Name;
        }

        protected override bool IsElementName(string elementName)
        {
            return !String.IsNullOrEmpty(elementName) && elementName == this.ElementName;
        }
    }
}