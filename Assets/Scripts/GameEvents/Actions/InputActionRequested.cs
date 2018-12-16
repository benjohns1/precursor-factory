using UnityEngine;

namespace GameEvents.Actions
{
    public class InputActionRequested : IEvent
    {
        public enum ActionType { SelectAtPosition, MultiSelectAtPosition, MoveToPosition, EnqueueMoveToPosition }

        public Vector2 Position { get; protected set; }
        public ActionType Action { get; protected set; }

        public InputActionRequested(Vector2 position, ActionType action)
        {
            Position = position;
            Action = action;
        }
    }
}