using GameEvents.Actions;
using GameEvents.UnitTask;
using System;
using System.Collections.Generic;
using UnitTask.Movement;
using UnityEngine;

namespace UnitTask
{
    [RequireComponent(typeof(SelectableComponent))]
    public class TaskQueue
    {
        protected Queue<ITask> Queue = new Queue<ITask>();

        internal ITask Current { get; private set; }
        internal SelectableComponent SelectableComponent;

        internal TaskQueueComponent TaskQueueComponent;

        internal bool Selected = false;

        public TaskQueue(SelectableComponent selectable, TaskQueueComponent queueComponent)
        {
            SelectableComponent = selectable;
            TaskQueueComponent = queueComponent;
        }

        internal void ProcessInputAction(InputActionRequested inputEvent)
        {
            if (!inputEvent.MultiModifier)
            {
                Queue.Clear();
                CancelCurrent();
            }
            switch (inputEvent.Action)
            {
                case InputActionRequested.ActionType.MoveToPosition:
                    Queue.Enqueue(new MoveToPosition(this, Camera.main.ScreenToWorldPoint(inputEvent.Position)));
                    break;
                case InputActionRequested.ActionType.Drill:
                    Queue.Enqueue(new MoveToPosition(this, Camera.main.ScreenToWorldPoint(inputEvent.Position)));
                    Queue.Enqueue(new Manufacture.Drill(this));
                    break;
            }
        }

        internal void ProcessBuildAction(BuildActionRequested buildEvent)
        {
            if (!buildEvent.MultiModifier)
            {
                Queue.Clear();
                CancelCurrent();
            }

            Queue.Enqueue(new MoveToPosition(this, Camera.main.ScreenToWorldPoint(buildEvent.Position)));

            switch (buildEvent.Building)
            {
                case BuildActionRequested.BuildingType.Drill:
                    // @TODO: create global build job/ghost and associate drone task to it (build system captures build request first, then fires another event afterwards on success?)
                    Queue.Enqueue(new Build.Drill(this));
                    Debug.Log("Enqueued build task for drone, @TODO: build system");
                    break;
                case BuildActionRequested.BuildingType.Refinery:
                    // @TODO: create global build job/ghost and associate drone task to it (build system captures build request first, then fires another event afterwards on success?)
                    Queue.Enqueue(new Build.Refinery(this));
                    Debug.Log("Enqueued build task for drone, @TODO: build system");
                    break;
                default:
                    Debug.LogWarning("Building " + buildEvent.Building.ToString() + " not handled");
                    break;
            }
        }

        internal void FinishCurrent()
        {
            Current = null;
        }

        internal void RunCurrent()
        {
            try
            {
                if (Current == null)
                {
                    if (Queue.Count <= 0)
                    {
                        return;
                    }
                    Current = Queue.Dequeue();
                    GameManager.EventSystem.Publish(new UnitTaskStarted(Current));
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                CancelCurrent();
            }
        }

        protected void CancelCurrent()
        {
            if (Current != null)
            {
                GameManager.EventSystem.Publish(new UnitTaskCancelled(Current));
                Current = null;
            }
        }
    }
}
