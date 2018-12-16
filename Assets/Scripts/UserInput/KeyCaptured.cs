using GameEvents;
using UnityEngine;

namespace UserInput
{
    class KeyCaptured : IEvent
    {
        public enum ActionType { Pressed, Released }

        protected Topic topic = new Topic("InputCapture");

        public KeyCode Key { get; protected set; }
        public ActionType Action { get; protected set; }

        public KeyCaptured(KeyCode key, ActionType action)
        {
            Key = key;
            Action = action;
        }

        public Topic GetTopic()
        {
            return topic;
        }
    }
}