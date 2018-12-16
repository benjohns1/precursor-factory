using System;
using System.Collections.Generic;
using GameEvents;
using Movement;
using UnityEngine;
using UserInput;

namespace UnitCommand
{
    public class CommandQueueComponent : MonoBehaviour
    {
        protected readonly Topic inputActionTopic = new Topic("InputAction");

        protected Queue<Command> queue = new Queue<Command>();

        protected Command current = null;

        protected bool running = false;

        protected SelectableComponent selectable;

        protected bool Selected = false;

        private void Awake()
        {
            selectable = gameObject.GetComponent<SelectableComponent>();
            if (selectable == null)
            {
                throw new Exception("Command queue component requires the object also is selectable");
            }
            GameManager.EventSystem.Subscribe(new Topic("UnitSelection"), HandleSelectionEvent);
        }

        protected void HandleSelectionEvent(IEvent @event)
        {
            if (GameManager.SelectionSystem.IsSelected(selectable))
            {
                if (!Selected)
                {
                    Selected = true;
                    GameManager.EventSystem.Subscribe(inputActionTopic, HandleInputEvent);
                }
            }
            else
            {
                Selected = false;
                GameManager.EventSystem.Unsubscribe(inputActionTopic, HandleInputEvent);
            }
        }

        protected void HandleInputEvent(IEvent @event)
        {
            if (!(@event is InputAction))
            {
                return;
            }

            InputAction inputEvent = @event as InputAction;
            switch (inputEvent.Action)
            {
                case InputAction.ActionType.MoveToPosition:
                    queue.Clear();
                    CancelCurrent();
                    queue.Enqueue(new CommandMoveToPosition(inputEvent.Position));
                    break;
                case InputAction.ActionType.EnqueueMoveToPosition:
                    queue.Enqueue(new CommandMoveToPosition(inputEvent.Position));
                    break;
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
