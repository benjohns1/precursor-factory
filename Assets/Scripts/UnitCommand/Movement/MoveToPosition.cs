using UnityEngine;

namespace UnitCommand.Movement
{
    public class MoveToPosition : ICommand
    {
        public CommandQueue CommandQueue { get; protected set; }
        public Vector2 TargetPosition { get; protected set; }

        public MoveToPosition(CommandQueue commandQueue, Vector2 targetPosition)
        {
            CommandQueue = commandQueue;
            TargetPosition = targetPosition;
        }
    }
}
