using Microsoft.Extensions.Configuration;

namespace Rhendaria.Hosting.Implementation
{
    public class RhendariaMySqlHostConfiguration : RhendariaHostConfigurationBase
    {
        public RhendariaMySqlHostConfiguration(IConfiguration configuration) : base(configuration)
        {
            ServiceName = configuration["ClusterId"];
            ClusterId = configuration["ServiceName"];
            ConnectionString = configuration["MyConnectionString"];
            SqlClientInvariant = configuration["MySqlClient"];
            SiloInteractionPort = configuration.GetValue<int>("SiloToSiloPort");
            GatewayPort = configuration.GetValue<int>("SiloGatewayPort");
            LogFile = configuration["LogFile"];
        }
    }
}