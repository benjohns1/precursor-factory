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

        internal void HandleAction(InputActionRequested inputEvent)
        {
            switch (inputEvent.Action)
            {
                case InputActionRequested.ActionType.MoveToPosition:
                    if (!inputEvent.MultiModifier)
                    {
                        Queue.Clear();
                        CancelCurrent();
                    }
                    Queue.Enqueue(new MoveToPosition(this, Camera.main.ScreenToWorldPoint(inputEvent.Position)));
                    break;
                case InputActionRequested.ActionType.Drill:
                    if (!inputEvent.MultiModifier)
                    {
                        Queue.Clear();
                        CancelCurrent();
                    }
                    Queue.Enqueue(new MoveToPosition(this, Camera.main.ScreenToWorldPoint(inputEvent.Position)));
                    Queue.Enqueue(new Manufacture.Drill(this));
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
