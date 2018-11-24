using System;

namespace Rhendaria.Abstraction.Services
{
    public interface IEventBus
    {
        void Publish(string topic, object message);
        void Subscribe(string topic, Action<object> handler);
    }
}