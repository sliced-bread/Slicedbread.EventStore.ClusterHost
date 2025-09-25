using System;

namespace HealthCheck.Models
{
    public class Member
    {
        public string InstanceId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string State { get; set; }
        public bool IsAlive { get; set; }
        public string InternalTcpIp { get; set; }
        public int InternalTcpPort { get; set; }
        public int InternalSecureTcpPort { get; set; }
        public string ExternalTcpIp { get; set; }
        public int ExternalTcpPort { get; set; }
        public int ExternalSecureTcpPort { get; set; }
        public string InternalHttpIp { get; set; }
        public int InternalHttpPort { get; set; }
        public string ExternalHttpIp { get; set; }
        public int ExternalHttpPort { get; set; }
    }
}