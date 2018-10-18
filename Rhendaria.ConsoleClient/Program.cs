using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Logging;

namespace Rhendaria.ConsoleClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new ClientBuilder()
                // Use localhost clustering for a single local silo
                .UseLocalhostClustering()
                // Configure ClusterId and ServiceId
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "MyAwesomeService";
                })
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.SetMinimumLevel(LogLevel.Information).AddFile("log.info.txt");
                    loggingBuilder.SetMinimumLevel(LogLevel.Debug).AddFile("log.debug.txt");
                });

            var client = builder.Build();
            await client.Connect();

            await Console.Out.WriteLineAsync("Connected");
        }
    }
}