﻿namespace Slicedbread.EventStore.ClusterHost.Configuration
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

        public InternalNode this[int index]
        {
            get
            {
                return this.BaseGet(index) as InternalNode;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new InternalNode();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((InternalNode)element).Name;
        }

        protected override bool IsElementName(string elementName)
        {
            return !String.IsNullOrEmpty(elementName) && elementName == this.ElementName;
        }
    }
}