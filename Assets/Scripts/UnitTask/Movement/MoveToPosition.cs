using UnityEngine;

namespace UnitTask.Movement
{
    public class MoveToPosition : ITask
    {
        public TaskQueue TaskQueue { get; protected set; }
        public Vector2 TargetPosition { get; protected set; }

        public MoveToPosition(TaskQueue taskQueue, Vector2 targetPosition)
        {
            TaskQueue = taskQueue;
            TargetPosition = targetPosition;
        }
    }
}
