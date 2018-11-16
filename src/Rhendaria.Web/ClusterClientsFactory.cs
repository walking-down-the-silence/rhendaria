using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Logging;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

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

            int currentAttempts = 0;
            int maximumAttempts = 10;
            Task<bool> RetryFunc(Exception exception) => Retry(currentAttempts++, maximumAttempts, exception);

            Task.WaitAll(client.Connect(RetryFunc));

            return client;
        }

        private async Task<bool> Retry(int currentAttempts, int maximumAttempts, Exception exception)
        {
            switch (exception)
            {
                case SiloUnavailableException ex:
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    return currentAttempts < maximumAttempts;
                default:
                    return false;
            }
        }
    }
}