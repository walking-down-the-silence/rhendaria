using Orleans.Hosting;
using System.Threading.Tasks;

namespace Rhendaria.Hosting
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            ISiloHostBuilder hostBuilder = new SiloHostBuilder()
                .ConfigureEndpoints(siloPort: 4000, gatewayPort: 6000);

            ISiloHost host = hostBuilder.Build();

            await host.StartAsync();
        }
    }
}
