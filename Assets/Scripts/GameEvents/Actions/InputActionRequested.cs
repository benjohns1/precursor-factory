using UnityEngine;

namespace GameEvents.Actions
{
    public class InputActionRequested : IEvent
    {
        public enum ActionType { None, ChooseAction, SelectAtPosition, MoveToPosition, Drill }

        public Vector2 Position { get; protected set; }
        public ActionType Action { get; protected set; }
        public bool MultiModifier { get; protected set; }

        public InputActionRequested(Vector2 position, ActionType action, bool multiModifier)
        {
            Position = position;
            Action = action;
            MultiModifier = multiModifier;
        }
    }
}