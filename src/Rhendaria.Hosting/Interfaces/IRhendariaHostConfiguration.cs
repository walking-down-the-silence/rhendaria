namespace Rhendaria.Hosting.Interfaces
{
    public interface IRhendariaHostConfiguration
    {
        string ServiceName { get; }
        string ClusterId { get; }
        string ConnectioString { get; }
        string SqlClietInvariant { get; }
        int SiloInteractionPort { get; }
        int GatewayPort { get; }
    }
}