using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rhendaria.Abstraction;

namespace Rhendaria.Hosting
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appconfig.json", optional: false);
            var config = configBuilder.Build();

            ISiloHostBuilder hostBuilder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "rhendaria.server.cluster";
                    options.ServiceId = "rhendaria.server.service";
                })
                .AddMemoryGrainStorageAsDefault()
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureServices(services =>
                {
                    services.Configure<ZoneOptions>(config.GetSection("ZoneOptions"));
                });

            ISiloHost host = hostBuilder.Build();

            await host.StartAsync();

            Console.WriteLine("Press any key to continue and close the server.");
            Console.ReadKey();
        }
    }
}
