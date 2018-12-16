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
            Topic topic = @event.GetTopic();
            List<Callback> subs = callbacks.Where(subscription => subscription.Topic.Equals(topic)).Select(subscription => subscription.Callback).ToList();
            foreach (Callback callback in subs)
            {
                callback.Invoke(@event);
            }
        }

        public void Subscribe(Topic topic, Callback callback)
        {
            callbacks.Add(new Subscription(topic, callback));
        }

        public void Unsubscribe(Topic topic, Callback callback)
        {
            Subscription subscription = new Subscription(topic, callback);
            if (!callbacks.Contains(subscription))
            {
                return;
            }
            callbacks.Remove(subscription);
        }
    }
}
