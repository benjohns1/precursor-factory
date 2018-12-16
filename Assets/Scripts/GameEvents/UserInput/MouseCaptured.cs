using UnityEngine;

namespace GameEvents.UserInput
{
    public class MouseCaptured : KeyCaptured
    {
        public Vector2 Position { get; protected set; }

        public MouseCaptured(KeyCode key, ActionType action, Vector3 mousePosition) : base(key, action)
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            Position = new Vector2(worldPoint.x, worldPoint.y);
        }
    }
}