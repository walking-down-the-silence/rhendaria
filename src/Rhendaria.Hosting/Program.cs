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
using Rhendaria.Hosting.Implemetation;

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

            await WriteAsync("Rhendaria host starting ......");

            try
            {
                var host = await new RhendariaHostFactory().StartNewAsync(configuration);

                await WriteLineAsync("[OK]", ConsoleColor.Green);

                await WriteLineAsync("Press any key to stop the host");
                await Console.In.ReadLineAsync();

                await host.StopAsync();
            }
            catch (Exception e)
            {
                await WriteLineAsync("[Failed]", ConsoleColor.Red);
                await WriteLineAsync(e.Message);

                await Console.In.ReadLineAsync();
            }
        }

        public static async Task WriteAsync(string text, ConsoleColor color = ConsoleColor.White)
        {
            var previusColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            await Console.Out.WriteAsync(text);
            Console.ForegroundColor = previusColor;
        }

        public static async Task WriteLineAsync(string text, ConsoleColor color = ConsoleColor.White)
        {
            await WriteAsync($"{text}{Environment.NewLine}", color);
        }
    }
}