using System;
using Orleans;

namespace Rhendaria.Abstraction
{
    public interface IEventBus : IGrainWithGuidKey
    {
        void Publish(string topic, object message);
        void Subscribe(string topic, Action<object> handler);
    }
}