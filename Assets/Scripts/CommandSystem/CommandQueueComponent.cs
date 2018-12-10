using System;
using System.Collections.Generic;
using EventSystem;
using MovementSystem;
using UnityEngine;

namespace CommandSystem
{
    public class CommandQueueComponent : MonoBehaviour
    {
        protected Queue<Command> queue = new Queue<Command>();

        protected Command current = null;

        protected bool running = false;

        private void Awake()
        {
            GameManager.EventSystem.Subscribe(new Topic("input"), HandleInputEvent);
        }

        protected void HandleInputEvent(IEventData @event)
        {
            InputEvent inputEvent = @event as InputEvent;
            if (inputEvent.Type == InputEvent.InputType.Secondary)
            {
                queue.Clear();
                CancelCurrent();
                queue.Enqueue(new CommandMoveToPosition(inputEvent.Position));
            }
            else if (inputEvent.Type == InputEvent.InputType.SecondaryShift)
            {
                queue.Enqueue(new CommandMoveToPosition(inputEvent.Position));
                Debug.Log("Queued commands: " + queue.Count);
            }
        }

        private void Update()
        {
            RunCurrentCommand();
        }

        protected void CancelCurrent()
        {
            if (current != null)
            {
                current.Stop(this);
                current = null;
            }
        }

        protected void RunCurrentCommand()
        {
            try
            {
                if (current == null)
                {
                    if (queue.Count <= 0)
                    {
                        return;
                    }
                    current = queue.Dequeue();
                    running = current.Start(this);
                }

                if (running)
                {
                    running = current.Update(this);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                running = false;
            }

            if (!running)
            {
                CancelCurrent();
            }
        }
    }
}
