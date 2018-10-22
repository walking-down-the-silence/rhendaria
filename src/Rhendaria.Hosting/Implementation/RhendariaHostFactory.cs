using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Rhendaria.Hosting.Implementation;
using Rhendaria.Hosting.Interfaces;

namespace Rhendaria.Hosting.Implemetation
{
    public class RhendariaHostFactory : IRhendariaHostFactory
    {
        public async Task<IRhendariaHost> StartMySqlHostAsync(IConfiguration configuration)
        {
            var hostCondConfiguration = new RhendariaMySqlHostConfiguration(configuration);

            var host = await StartNewHostAsync(hostCondConfiguration);
            return host;
        }

        public async Task<IRhendariaHost> StartPosgressHostAsync(IConfiguration configuration)
        {
            var hostCondConfiguration = new RhendariaPostgressHostConfiguration(configuration);

            var host = await StartNewHostAsync(hostCondConfiguration);
            return host;
        }

        public async Task<IRhendariaHost> StartNewHostAsync(IRhendariaHostConfiguration configuration)
        {
            var host = new RhendariaHost(configuration);
            await host.StartAsync();

            return host;
        }
    }
}