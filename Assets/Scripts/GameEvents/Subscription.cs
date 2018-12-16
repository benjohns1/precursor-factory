using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEvents
{
    internal struct Subscription
    {
        internal readonly Topic Topic;
        internal readonly EventSystem.Callback Callback;

        public Subscription(Topic topic, EventSystem.Callback callback)
        {
            Topic = topic;
            Callback = callback;
        }
    }
}
