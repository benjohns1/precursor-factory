using UnityEngine;

namespace GameEvents.UserInput
{
    public class MouseEventCaptured : KeyCaptured
    {
        public Vector3 Position { get; protected set; }
        public bool GameMouseMode { get; protected set; }

        public MouseEventCaptured(KeyCode key, ActionType action, Vector3 mousePosition, bool gameMouseMode) : base(key, action)
        {
            Position = mousePosition;
            GameMouseMode = gameMouseMode;
        }
    }
}