﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.ApplicationParts;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Logging;
using Rhendaria.Abstraction;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Abstraction.Services;
using Rhendaria.Engine.Actors;
using Rhendaria.Engine.Services;
using Rhendaria.Hosting.Interfaces;

namespace Rhendaria.Hosting.Implementation
{
    public class RhendariaHost : IRhendariaHost
    {
        public RhendariaHostOptions Configuration { get; }
        public ZoneOptions ZoneOptions { get; }

        private ISiloHost _silo;

        public RhendariaHost(IOptions<RhendariaHostOptions> hostOptions, IOptions<ZoneOptions> zonOptions)
        {
            ZoneOptions = zonOptions.Value;
            Configuration = hostOptions.Value;
        }

        public async Task StartAsync()
        {
            if (_silo != null)
                throw new InvalidOperationException("Host is already started");

            _silo = BuildSilo();
            await _silo.StartAsync();
        }

        public async Task StopAsync()
        {
            if (_silo == null)
                throw new InvalidOperationException("Host is not started yet");

            await _silo.StopAsync();
            _silo = null;
        }

        private ISiloHost BuildSilo()
        {
            var silo = new SiloHostBuilder()
                .UseDashboard()
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
                .ConfigureEndpoints(Configuration.SiloInteractionPort, Configuration.GatewayPort,listenOnAnyHostAddress: true)
                .ConfigureServices(services =>
                {
                    services.Configure<ZoneOptions>(options =>
                    {
                        options.ZoneHeight = ZoneOptions.ZoneHeight;
                        options.ZoneWidth = ZoneOptions.ZoneWidth;
                    });

                    services.AddSingleton<IEventBus, EventBus>();
                    services.AddScoped<IScoreCalculatingService, ScoreCalculationService>();
                    services.AddScoped<ICollisionDetectingService, CollisionDetectingService>();
                })
                .ConfigureLogging(s =>
                {
                    s.SetMinimumLevel(LogLevel.Information).AddFile(Configuration.LogFile);
                })
                .ConfigureApplicationParts(appParts =>
                {
                    appParts
                        .AddApplicationPart(typeof(IPlayerActor).Assembly)
                        .AddApplicationPart(typeof(PlayerActor).Assembly);
                })
                .Build();

            return silo;
        }
    }
}