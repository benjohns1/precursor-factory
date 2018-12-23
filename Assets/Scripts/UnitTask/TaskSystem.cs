using GameEvents;
using GameEvents.Actions;
using GameEvents.UnitSelection;
using GameEvents.UnitTask;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnitTask
{
    public class TaskSystem
    {
        List<TaskQueue> queues = new List<TaskQueue>();

        public TaskSystem()
        {
            GameManager.EventSystem.Subscribe(typeof(InputActionRequested), HandleInputActionEvent);
            GameManager.EventSystem.Subscribe(typeof(BuildActionRequested), HandleBuildActionEvent);
            GameManager.EventSystem.Subscribe(typeof(UnitSelectionChanged), HandleSelectionEvent);
            GameManager.EventSystem.Subscribe(typeof(UnitTaskCompleted), HandleCompletedEvent);
        }

        private void HandleCompletedEvent(IEvent @event)
        {
            UnitTaskCompleted completedEvent = @event as UnitTaskCompleted;
            TaskQueue completedQueue = queues.FirstOrDefault(queue => queue.Current == completedEvent.Task);
            if (completedQueue == default(TaskQueue))
            {
                Debug.LogWarning("No matching " + typeof(TaskQueue).ToString() + " when processing " + typeof(UnitTaskCompleted).ToString());
                return;
            }
            completedQueue.FinishCurrent();
        }

        private void HandleSelectionEvent(IEvent @event)
        {
            List<SelectableComponent> selections = GameManager.SelectionSystem.GetSelections();
            foreach (TaskQueue queue in queues)
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

        private void HandleInputActionEvent(IEvent @event)
        {
            InputActionRequested inputEvent = @event as InputActionRequested;

            List<SelectableComponent> selections = GameManager.SelectionSystem.GetSelections();
            foreach (TaskQueue queue in queues.Where(queue => selections.Contains(queue.SelectableComponent)))
            {
                queue.ProcessInputAction(inputEvent);
            }
        }

        private void HandleBuildActionEvent(IEvent @event)
        {
            BuildActionRequested buildEvent = @event as BuildActionRequested;

            List<SelectableComponent> selections = GameManager.SelectionSystem.GetSelections();
            foreach (TaskQueue queue in queues.Where(queue => selections.Contains(queue.SelectableComponent)))
            {
                queue.ProcessBuildAction(buildEvent);
            }
        }

        public void Register(TaskQueue queue)
        {
            if (!queues.Contains(queue))
            {
                queues.Add(queue);
            }
        }

        public void Unregister(TaskQueue queue)
        {
            if (queues.Contains(queue))
            {
                queues.Remove(queue);
            }
        }

        public void Update()
        {
            foreach (TaskQueue queue in queues)
            {
                queue.RunCurrent();
            }
        }
    }
}
