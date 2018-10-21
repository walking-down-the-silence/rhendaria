using Microsoft.Extensions.Configuration;
using Rhendaria.Hosting.Interfaces;

namespace Rhendaria.Hosting.Implemetation
{
    public class RhendariaHostConfiguration : IRhendariaHostConfiguration
    {
        public RhendariaHostConfiguration(IConfiguration configuration)
        {
            ServiceName = configuration["ClusterId"];
            ClusterId = configuration["ServiceName"];
            ConnectioString = configuration["ConnectionString"];
            SqlClietInvariant = configuration["SqlClient"];
            SiloInteractionPort = configuration.GetValue<int>("SiloToSiloPort");
            GatewayPort = configuration.GetValue<int>("SiloGatewayPort");
        }

        public RhendariaHostConfiguration(string serviceName, string clusterId, string connectioString, string sqlClietInvariant, int siloInteractionPort, int gatewayPort)
        {
            ServiceName = serviceName;
            ClusterId = clusterId;
            ConnectioString = connectioString;
            SqlClietInvariant = sqlClietInvariant;
            SiloInteractionPort = siloInteractionPort;
            GatewayPort = gatewayPort;
        }

        public string ServiceName { get; }
        public string ClusterId { get; }
        public string ConnectioString { get; }
        public string SqlClietInvariant { get; }
        public int SiloInteractionPort { get; }
        public int GatewayPort { get; }
    }
}