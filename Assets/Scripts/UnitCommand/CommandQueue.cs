using GameEvents.Actions;
using GameEvents.UnitCommand;
using System;
using System.Collections.Generic;
using UnitCommand.Movement;
using UnityEngine;

namespace UnitCommand
{
    [RequireComponent(typeof(SelectableComponent))]
    public class CommandQueue
    {
        protected Queue<ICommand> Queue = new Queue<ICommand>();

        internal ICommand Current { get; private set; }
        internal SelectableComponent SelectableComponent;

        internal CommandQueueComponent CommandQueueComponent;

        internal bool Selected = false;

        public CommandQueue(SelectableComponent selectable, CommandQueueComponent queueComponent)
        {
            SelectableComponent = selectable;
            CommandQueueComponent = queueComponent;
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
                    Queue.Enqueue(new Drill.Drill(this));
                    break;
            }
        }

        internal void FinishCurrentCommand()
        {
            Current = null;
        }

        internal void RunCurrentCommand()
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
                    GameManager.EventSystem.Publish(new UnitCommandStarted(Current));
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
                GameManager.EventSystem.Publish(new UnitCommandCancelled(Current));
                Current = null;
            }
        }
    }
}
