using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;
using Rhendaria.Engine;
using Orleans;

namespace Rhendaria.Hosting
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();


            try
            {
                var host = BuildSilo(configuration);
                await host.StartAsync();
                Console.WriteLine("Press Enter to terminate...");
                Console.ReadLine();

                await host.StopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static ISiloHost BuildSilo(IConfiguration configuration)
        {
            var silo = new SiloHostBuilder()
                // Clustering information
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = configuration["ClusterId"];
                    options.ServiceId = configuration["ServiceName"];
                })
                .UseAdoNetClustering(options =>
                {
                    options.Invariant = "MySql.Data.MySqlClient";
                    options.ConnectionString = configuration["ConnectionString"];
                })
                // Endpoints
                .ConfigureEndpoints(configuration.GetValue<int>("SiloToSiloPort"), configuration.GetValue<int>("SiloGatewayPort"))
                // Application parts: just reference one of the grain implementations that we use
                //.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(RhendariaGrain).Assembly).WithReferences())
                // Now create the silo!
                .Build();

            return silo;
        }
    }
}