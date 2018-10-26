using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Rhendaria.Hosting
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            ISiloHostBuilder hostBuilder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "rhendaria.server.cluster";
                    options.ServiceId = "rhendaria.server.service";
                })
                .AddMemoryGrainStorageAsDefault()
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback);

            ISiloHost host = hostBuilder.Build();

            await host.StartAsync();

            Console.WriteLine("Press any key to continue and close the server.");
            Console.ReadKey();
        }
    }
}
