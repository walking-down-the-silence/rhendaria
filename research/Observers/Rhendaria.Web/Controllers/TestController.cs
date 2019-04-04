using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Rhendaria.Abstraction.Actors;

namespace RhendariaObserver.Web.Controllers
{
    public class TestController
    {
        private readonly IClusterClient _client;

        public TestController(IClusterClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task AddCounter()
        {
            var grain = _client.GetGrain<IMessageContainer>("0");
            await grain.InsertMessage();
        }
    }
}
