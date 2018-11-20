using System;
using System.Collections.Generic;
using Rhendaria.Abstraction;

namespace Rhendaria.Engine
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<string, List<Action<object>>> _handlers = new Dictionary<string, List<Action<object>>>();

        public void Publish(string topic, object message)
        {
            if (_handlers.TryGetValue(topic, out var handlers))
                handlers.ForEach(h => h(message));
        }

        public void Subscribe(string topic, Action<object> handler)
        {
            if(!_handlers.ContainsKey(topic))
                _handlers[topic] = new List<Action<object>>();

            _handlers[topic].Add(handler);
        }
    }
}