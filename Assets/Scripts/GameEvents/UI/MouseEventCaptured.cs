using UnityEngine;

namespace GameEvents.UI
{
    public class MouseEventCaptured : IEvent
    {
        public KeyCode Key { get; protected set; }
        public Vector3 Position { get; protected set; }

        public MouseEventCaptured(KeyCode key, Vector3 mousePosition)
        {
            Position = mousePosition;
            Key = key;
        }
    }
}