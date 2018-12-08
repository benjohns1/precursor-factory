using System;
using System.Collections.Generic;
using System.Linq;

namespace EventSystem
{
    public class EventSystem
    {
        public delegate void Callback(IEventData @event);

        protected List<IEventData> events = new List<IEventData>();
        protected List<Tuple<Topic, Callback>> callbacks = new List<Tuple<Topic, Callback>>();

        public void Add(IEventData @event)
        {
            events.Add(@event);
            Publish(@event);
        }

        public void Subscribe(Topic topic, Callback callback)
        {
            callbacks.Add(new Tuple<Topic, Callback>(topic, callback));
        }

        public void Unsubscribe(Topic topic, Callback callback)
        {
            callbacks.Remove(new Tuple<Topic, Callback>(topic, callback));
        }

        protected void Publish(IEventData @event)
        {
            Topic topic = @event.GetTopic();
            foreach (Callback callback in callbacks.Where(item => item.Item1 == topic).Select(item => item.Item2))
            {
                callback.BeginInvoke(@event, null, null);
            }
        }
    }
}
