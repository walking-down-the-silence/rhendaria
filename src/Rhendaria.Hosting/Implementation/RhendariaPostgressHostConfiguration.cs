using Microsoft.Extensions.Configuration;

namespace Rhendaria.Hosting.Implementation
{
    public class RhendariaPostgressHostConfiguration : RhendariaHostConfigurationBase
    {
        public RhendariaPostgressHostConfiguration(IConfiguration configuration) : base(configuration)
        {
            ServiceName = configuration["ClusterId"];
            ClusterId = configuration["ServiceName"];
            ConnectionString = configuration["PostgressConnectionString"];
            SqlClientInvariant = configuration["PostgressClient"];
            SiloInteractionPort = configuration.GetValue<int>("SiloToSiloPort");
            GatewayPort = configuration.GetValue<int>("SiloGatewayPort");
            LogFile = configuration["LogFile"];
        }
    }
}