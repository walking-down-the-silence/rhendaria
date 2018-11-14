namespace Rhendaria.Hosting.Implementation
{
    public class RhendariaHostConfigurationOptions
    {
        public string ServiceName { get; set; }
        public string ClusterId { get; set; }
        public string ConnectionString { get; set; }
        public string SqlClientInvariant { get; set; }
        public int SiloInteractionPort { get; set; }
        public int GatewayPort { get; set; }
        public string LogFile { get; set; }
    }
}