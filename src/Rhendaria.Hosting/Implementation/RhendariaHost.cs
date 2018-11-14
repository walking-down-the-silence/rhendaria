﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Logging;
using Rhendaria.Hosting.Interfaces;

namespace Rhendaria.Hosting.Implementation
{
    public class RhendariaHost : IRhendariaHost
    {
        private ISiloHost _silo;
        public RhendariaHostConfigurationOptions Configuration { get; }

        public RhendariaHost(IOptions<RhendariaHostConfigurationOptions> configuration)
        {
            Configuration = configuration.Value;
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
                .AddAdoNetGrainStorageAsDefault(options =>
                {
                    options.ConnectionString = Configuration.ConnectionString;
                    options.Invariant = Configuration.SqlClientInvariant;
                    options.UseJsonFormat = true;
                })
                .ConfigureEndpoints(Configuration.SiloInteractionPort, Configuration.GatewayPort, listenOnAnyHostAddress: true)
                .ConfigureLogging(s =>
                {
                    s.SetMinimumLevel(LogLevel.Information).AddFile(Configuration.LogFile);
                })
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