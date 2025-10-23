namespace HealthCheck.Models
{
    public class ClusterStatus
    {
        public Member[] Members { get; set; }
        public string ServerIp { get; set; }
        public int ServerPort { get; set; }
    }
}