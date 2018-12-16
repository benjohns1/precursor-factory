using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameEvents
{
    internal struct Subscription
    {
        internal readonly Type EventType;
        internal readonly EventSystem.Callback Callback;

        public Subscription(Type eventType, EventSystem.Callback callback)
        {
            EventType = eventType;
            Callback = callback;
        }
    }
}
