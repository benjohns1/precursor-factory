using GameEvents.Actions;
using System.Collections.Generic;
using UnitTask.Manufacture;
using UnityEngine;

namespace UnitTask
{
    class AvailableTasksComponent : MonoBehaviour
    {
        public TaskType[] TaskTypes = new TaskType[0];

        public TaskType[] GetTasks(Vector2 mousePosition)
        {
            List<TaskType> tasks = new List<TaskType>();
            foreach (TaskType task in TaskTypes)
            {
                switch (task.Action)
                {
                    case InputActionRequested.ActionType.Drill:
                        bool found = Drill.GetDrillableAsteroid(Camera.main.ScreenToWorldPoint(mousePosition)) != default(AsteroidComponent);
                        if (found)
                        {
                            tasks.Add(task);
                        }
                        break;
                    default:
                        tasks.Add(task);
                        break;
                }
            }
            return tasks.ToArray();
        }
    }
}
