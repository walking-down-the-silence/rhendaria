using System.Threading.Tasks;

namespace Rhendaria.Hosting.Interfaces
{
    public interface IRhendariaHost
    {
        Task StartAsync();
        Task StopAsync();
    }
}