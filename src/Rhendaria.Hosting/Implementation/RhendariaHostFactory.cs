using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Rhendaria.Hosting.Interfaces;

namespace Rhendaria.Hosting.Implemetation
{
    public class RhendariaHostFactory : IRhendariaHostFactory
    {
        public async Task<IRhendariaHost> StartNewAsync(IConfiguration configuration)
        {
            var hostCondConfiguration = new RhendariaHostConfiguration(configuration);
            
            var host = new RhendariaMySqlHost(hostCondConfiguration);
            await host.StartAsync();

            return host;
        }
    }
}