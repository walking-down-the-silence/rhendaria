using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Logging;
using Orleans.Runtime;

namespace Rhendaria.Web
{
    public class ClusterClientsFactory
    {
        private readonly IConfiguration _configuration;

        public ClusterClientsFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IClusterClient CreateInstance()
        {
            var clientBuilder = new ClientBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = _configuration["ClusterId"];
                    options.ServiceId = _configuration["ServiceName"];
                })
                .UseAdoNetClustering(options =>
                {
                    options.ConnectionString = _configuration["ConnectingString"];
                    options.Invariant = _configuration["Invariant"];
                })
                .ConfigureLogging(builder =>
                {
                    builder.SetMinimumLevel(LogLevel.Debug).AddFile(_configuration["LogFile"]);
                });

            IClusterClient client = clientBuilder.Build();

            Task.WaitAll(client.Connect(new RetryState().ShouldRetry));

            return client;
        }

        private class RetryState
        {
            short _attempts;
            const short MaximumAllowedAttempts = 10;

            internal async Task<bool> ShouldRetry(Exception arg)
            {
                if (arg is SiloUnavailableException)
                {
                    _attempts++;
                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
                else
                    return false;

                return _attempts < MaximumAllowedAttempts;
            }
        }
    }
}