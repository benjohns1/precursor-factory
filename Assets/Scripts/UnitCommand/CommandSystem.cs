using GameEvents;
using GameEvents.Actions;
using GameEvents.UnitCommand;
using GameEvents.UnitSelection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnitCommand
{
    public class CommandSystem
    {
        List<CommandQueue> queues = new List<CommandQueue>();

        public CommandSystem()
        {
            GameManager.EventSystem.Subscribe(typeof(InputActionRequested), HandleInputEvent);
            GameManager.EventSystem.Subscribe(typeof(UnitSelectionChanged), HandleSelectionEvent);
            GameManager.EventSystem.Subscribe(typeof(UnitCommandCompleted), HandleCompletedEvent);
        }

        private void HandleCompletedEvent(IEvent @event)
        {
            UnitCommandCompleted completedEvent = @event as UnitCommandCompleted;
            CommandQueue completedQueue = queues.First(queue => queue.Current == completedEvent.Command);
            if (completedQueue == null)
            {
                Debug.LogWarning("No matching CommandQueue when processing UnitCommandComleted");
                return;
            }
            completedQueue.FinishCurrentCommand();
        }

        private void HandleSelectionEvent(IEvent @event)
        {
            List<SelectableComponent> selections = GameManager.SelectionSystem.GetSelections();
            foreach (CommandQueue queue in queues)
            {
                if (selections.Contains(queue.SelectableComponent))
                {
                    if (!queue.Selected)
                    {
                        queue.Selected = true;
                    }
                }
                else
                {
                    queue.Selected = false;
                }
            }
        }

        private void HandleInputEvent(IEvent @event)
        {
            InputActionRequested inputEvent = @event as InputActionRequested;

            List<SelectableComponent> selections = GameManager.SelectionSystem.GetSelections();
            foreach (CommandQueue queue in queues.Where(queue => selections.Contains(queue.SelectableComponent)))
            {
                queue.HandleAction(inputEvent);
            }
        }

        public void Register(CommandQueue queue)
        {
            if (!queues.Contains(queue))
            {
                queues.Add(queue);
            }
        }

        public void Unregister(CommandQueue queue)
        {
            if (queues.Contains(queue))
            {
                queues.Remove(queue);
            }
        }

        public void Update()
        {
            foreach (CommandQueue queue in queues)
            {
                queue.RunCurrentCommand();
            }
        }
    }
}
