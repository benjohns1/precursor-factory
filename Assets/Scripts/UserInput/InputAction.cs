using GameEvents;
using UnityEngine;

namespace UserInput
{
    class InputAction : IEvent
    {
        public enum ActionType { SelectAtPosition, MultiSelectAtPosition, MoveToPosition, EnqueueMoveToPosition }

        protected Topic topic = new Topic("InputAction");

        public Vector2 Position { get; protected set; }
        public ActionType Action { get; protected set; }

        public InputAction(Vector2 position, ActionType action)
        {
            Position = position;
            Action = action;
        }

        public Topic GetTopic()
        {
            return topic;
        }
    }
}