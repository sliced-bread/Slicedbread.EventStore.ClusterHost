namespace HealthCheck.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    [XmlRoot("configuration")]
    public class ClusterHostConfig
    {
        [XmlElement("eventStore")]
        public EventStore EventStore { get; set; }
    }

    public class ConfigSections
    {
        [XmlElement("section")]
        public List<Section> Sections { get; set; }
    }

    public class Section
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }
    }

    public class EventStore
    {
        [XmlAttribute("restartDelayMs")]
        public int RestartDelayMs { get; set; }

        [XmlAttribute("restartWindowMs")]
        public int RestartWindowMs { get; set; }

        [XmlAttribute("additionalFlags")]
        public string AdditionalFlags { get; set; }

        [XmlElement("internalNodes")]
        public InternalNodes InternalNodes { get; set; }

        [XmlElement("externalNodes")]
        public ExternalNodes ExternalNodes { get; set; }
    }

    public class InternalNodes
    {
        [XmlElement("node")]
        public List<Node> Nodes { get; set; }
    }

    public class ExternalNodes
    {
        // Empty element, can be extended if needed
    }

    public class Node
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("internalIpAddress")]
        public string InternalIpAddress { get; set; }

        [XmlAttribute("externalIpAddress")]
        public string ExternalIpAddress { get; set; }

        [XmlAttribute("intTcpPort")]
        public int IntTcpPort { get; set; }

        [XmlAttribute("intHttpPort")]
        public int IntHttpPort { get; set; }

        [XmlAttribute("extTcpPort")]
        public int ExtTcpPort { get; set; }

        [XmlAttribute("extHttpPort")]
        public int ExtHttpPort { get; set; }

        [XmlAttribute("httpPrefix")]
        public string HttpPrefix { get; set; }

        [XmlAttribute("dbPath")]
        public string DbPath { get; set; }

        [XmlAttribute("logPath")]
        public string LogPath { get; set; }

        [XmlAttribute("additionalFlags")]
        public string AdditionalFlags { get; set; }
    }

    public class NLogConfig
    {
        [XmlElement("targets")]
        public Targets Targets { get; set; }

        [XmlElement("rules")]
        public Rules Rules { get; set; }
    }

    public class Targets
    {
        [XmlElement("target")]
        public List<Target> TargetList { get; set; }
    }

    [XmlRoot(ElementName = "target", Namespace = "http://www.nlog-project.org/schemas/NLog.xsd")]
    public class Target
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "type", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string XsiType { get; set; }

        [XmlAttribute("fileName")]
        public string FileName { get; set; }

        [XmlAttribute("deleteOldFileOnStartup")]
        public bool DeleteOldFileOnStartup { get; set; }
    }
    
    public class Rules
    {
        [XmlElement("logger")]
        public List<Logger> Loggers { get; set; }
    }

    public class Logger
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("minlevel")]
        public string MinLevel { get; set; }

        [XmlAttribute("writeTo")]
        public string WriteTo { get; set; }
    }

    public class Startup
    {
        [XmlElement("supportedRuntime")]
        public SupportedRuntime SupportedRuntime { get; set; }
    }

    public class SupportedRuntime
    {
        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlAttribute("sku")]
        public string Sku { get; set; }
    }

}