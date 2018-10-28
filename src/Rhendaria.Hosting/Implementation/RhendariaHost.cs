using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Logging;
using Rhendaria.Engine;
using Rhendaria.Hosting.Interfaces;

namespace Rhendaria.Hosting.Implementation
{
    public class RhendariaHost : IRhendariaHost
    {
        private ISiloHost _silo;
        public IRhendariaHostConfiguration Configuration { get; }

        public RhendariaHost(IRhendariaHostConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task StartAsync()
        {
            if (_silo != null)
                throw new InvalidOperationException("Host is already started");

            _silo = BuildSilo();
            await _silo.StartAsync();
        }

        private ISiloHost BuildSilo()
        {
            var silo = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = Configuration.ClusterId;
                    options.ServiceId = Configuration.ServiceName;
                })
                .UseAdoNetClustering(options =>
                {
                    options.ConnectionString = Configuration.ConnectionString;
                    options.Invariant = Configuration.SqlClientInvariant;
                })
                .AddAdoNetGrainStorage("OrleansStorage", options =>
                {
                    options.ConnectionString = Configuration.ConnectionString;
                    options.Invariant = Configuration.SqlClientInvariant;
                    options.UseJsonFormat = true;
                })
                .ConfigureEndpoints(Configuration.SiloInteractionPort, Configuration.GatewayPort)
                .ConfigureLogging(s => s.SetMinimumLevel(LogLevel.Information).AddFile(Configuration.LogFile))
                .Build();

            return silo;
        }

        public async Task StopAsync()
        {
            if (_silo == null)
                throw new InvalidOperationException("Host is not started yet");

            await _silo.StopAsync();
            _silo = null;
        }
    }
}