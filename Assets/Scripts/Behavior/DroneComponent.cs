using GameEvents.Actions;
using System.Collections.Generic;
using System.Linq;
using UnitTask.Manufacture;
using UnityEngine;

namespace Behavior
{
    class DroneComponent : BehaviorComponent
    {
        public string DroneName = null;

        public override string DisplayName => string.IsNullOrWhiteSpace(DroneName) ? "Drone" : DroneName;
        public override string DisplayType => "Drone";

        public override int MaxCargoVolume => 10;

        public override TaskType[] GetTasks(Vector2 mousePosition)
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
