using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Rhendaria.Abstraction.Actors;

namespace RhendariaObserver.Web.Controllers
{
    public class TestController
    {
        private readonly IClusterClient _client;
        private static IClientEventListener Listener;
        
        public TestController(IClusterClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task AddCounter()
        {
            var grain = _client.GetGrain<IMessageContainer>("0");
            if (Listener == null)
            {
                var chat = new ClientEventListener();

                IClientEventListener ref1 = await _client.CreateObjectReference<IClientEventListener>(chat);
                Listener = ref1;
                await grain.Subscribe(ref1);
            }

            await grain.InsertMessage("message");
        }
    }

    public class ClientEventListener : IClientEventListener
    {
        public void PushMessage(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
