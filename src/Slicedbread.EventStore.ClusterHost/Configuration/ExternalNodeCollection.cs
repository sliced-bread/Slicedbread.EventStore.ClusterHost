namespace Slicedbread.EventStore.ClusterHost.Configuration
{
    using System;
    using System.Configuration;

    public class ExternalNodeCollection : ConfigurationElementCollection
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

        public ExternalNode this[int index]
        {
            get
            {
                return this.BaseGet(index) as ExternalNode;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ExternalNode();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExternalNode)element).Name;
        }

        protected override bool IsElementName(string elementName)
        {
            return !String.IsNullOrEmpty(elementName) && elementName == this.ElementName;
        }
    }
}