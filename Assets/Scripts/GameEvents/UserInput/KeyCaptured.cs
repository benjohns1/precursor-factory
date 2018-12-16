using UnityEngine;

namespace GameEvents.UserInput
{
    public class KeyCaptured : InputCaptured
    {
        public enum ActionType { Pressed, Released }

        public KeyCode Key { get; protected set; }
        public ActionType Action { get; protected set; }

        public KeyCaptured(KeyCode key, ActionType action)
        {
            Key = key;
            Action = action;
        }
    }
}
