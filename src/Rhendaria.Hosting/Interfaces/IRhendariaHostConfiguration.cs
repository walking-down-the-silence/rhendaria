namespace Rhendaria.Hosting.Interfaces
{
    public interface IRhendariaHostConfiguration
    {
        string ServiceName { get; }
        string ClusterId { get; }
        string ConnectionString { get; }
        string SqlClientInvariant { get; }
        int SiloInteractionPort { get; }
        int GatewayPort { get; }
        string LogFile { get; }
    }
}