using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Logging;

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

            Task.WaitAll(client.Connect());

            return client;
        }
    }
}