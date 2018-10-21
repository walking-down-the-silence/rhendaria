using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Rhendaria.Hosting.Interfaces
{
    public interface IRhendariaHostFactory
    {
        Task<IRhendariaHost> StartNewAsync(IConfiguration configuration);
    }
}