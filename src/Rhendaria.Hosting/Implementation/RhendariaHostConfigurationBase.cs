using Microsoft.Extensions.Configuration;
using Rhendaria.Hosting.Interfaces;

namespace Rhendaria.Hosting.Implementation
{
    public abstract class RhendariaHostConfigurationBase : IRhendariaHostConfiguration
    {
        protected RhendariaHostConfigurationBase(IConfiguration configuration)
        {
        }

        public string ServiceName { get; protected set; }
        public string ClusterId { get; protected set; }
        public string ConnectioString { get; protected set; }
        public string SqlClietInvariant { get; protected set; }
        public int SiloInteractionPort { get; protected set; }
        public int GatewayPort { get; protected set; }
        public string LogFile { get; protected set; }
    }
}