using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEvents
{
    public class EventSystem
    {
        public delegate void Callback(IEvent @event);

        protected List<IEvent> events = new List<IEvent>();
        private List<Subscription> callbacks = new List<Subscription>();

        public void Publish(IEvent @event)
        {
            events.Add(@event);
            List<Callback> subCallbacks = callbacks.Where(sub => sub.EventType.IsAssignableFrom(@event.GetType())).Select(sub => sub.Callback).ToList();
            foreach (Callback callback in subCallbacks)
            {
                callback.Invoke(@event);
            }
        }

        public void Subscribe(Type type, Callback callback)
        {
            callbacks.Add(new Subscription(type, callback));
        }

        public void Unsubscribe(Type type, Callback callback)
        {
            Subscription subscription = new Subscription(type, callback);
            if (!callbacks.Contains(subscription))
            {
                return;
            }
            callbacks.Remove(subscription);
        }
    }
}
