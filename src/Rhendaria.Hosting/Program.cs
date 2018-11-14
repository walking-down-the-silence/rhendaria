using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rhendaria.Hosting.Implementation;
using Rhendaria.Hosting.Interfaces;

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

            var serviceProvider = new ServiceCollection()
                .AddOptions()
                .AddSingleton<IRhendariaHost, RhendariaHost>()
                .Configure<RhendariaHostConfigurationOptions>(configuration.GetSection("RhendariaHostConfigurationOption"))
                .BuildServiceProvider();

            await WriteAsync("Rhendaria host starting ......");

            try
            {
                var host = serviceProvider.GetService<IRhendariaHost>();

                await host.StartAsync();
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