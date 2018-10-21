using System;
using System.Threading.Tasks;
using Orleans.Configuration;
using Orleans.Hosting;
using Rhendaria.Hosting.Interfaces;

namespace Rhendaria.Hosting.Implemetation
{
    public class RhendariaMySqlHost : IRhendariaHost
    {
        private ISiloHost _silo;
        public IRhendariaHostConfiguration Configuration { get; }

        public RhendariaMySqlHost(IRhendariaHostConfiguration configuration)
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
                    options.Invariant = Configuration.SqlClietInvariant;
                    options.ConnectionString = Configuration.ConnectioString;
                })
                // Endpoints
                .ConfigureEndpoints(Configuration.SiloInteractionPort, Configuration.GatewayPort)
                // Application parts: just reference one of the grain implementations that we use
                //.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(RhendariaGrain).Assembly).WithReferences())
                // Now create the silo!
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