using Microsoft.Extensions.Configuration;

namespace Rhendaria.Hosting.Implementation
{
    public class RhendariaPostgressHostConfiguration : RhendariaHostConfigurationBase
    {
        public RhendariaPostgressHostConfiguration(IConfiguration configuration) : base(configuration)
        {
            ServiceName = configuration["ServiceName"];
            ClusterId = configuration["ClusterId"];
            ConnectionString = configuration["PostgressConnectionString"];
            SqlClientInvariant = configuration["PostgressClient"];
            SiloInteractionPort = configuration.GetValue<int>("SiloToSiloPort");
            GatewayPort = configuration.GetValue<int>("SiloGatewayPort");
            LogFile = configuration["LogFile"];
        }
    }
}