namespace HealthCheck.Models
{
    public class HeathCheckConfiguration
    {
        public string ListenAddress { get; set; }
        public Node EventStoreNode { get; set; }
    }
}