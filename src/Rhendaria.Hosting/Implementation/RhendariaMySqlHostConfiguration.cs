using Microsoft.Extensions.Configuration;
using Rhendaria.Hosting.Implementation;

namespace Rhendaria.Hosting.Implemetation
{
    public class RhendariaMySqlHostConfiguration : RhendariaHostConfigurationBase
    {
        public RhendariaMySqlHostConfiguration(IConfiguration configuration) : base(configuration)
        {
            ServiceName = configuration["ClusterId"];
            ClusterId = configuration["ServiceName"];
            ConnectioString = configuration["MyConnectionString"];
            SqlClietInvariant = configuration["MySqlClient"];
            SiloInteractionPort = configuration.GetValue<int>("SiloToSiloPort");
            GatewayPort = configuration.GetValue<int>("SiloGatewayPort");
            LogFile = configuration["LogFile"];
        }
    }
}