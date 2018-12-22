using GameEvents.Actions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Behavior
{
    class DroneComponent : BehaviorComponent
    {
        public string DroneName;

        public override string DisplayName => string.IsNullOrWhiteSpace(DroneName) ? "Drone" : DroneName;
        public override string DisplayType => "Drone";

        public override CommandType[] GetCommands(Vector2 mousePosition)
        {
            List<CommandType> commands = new List<CommandType>();
            foreach (CommandType command in CommandTypes)
            {
                switch (command.Action)
                {
                    case InputActionRequested.ActionType.Drill:
                        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                        bool found = Physics.RaycastAll(ray, Mathf.Infinity).Any(h => h.transform?.gameObject.GetComponent<AsteroidComponent>() != null);
                        if (found)
                        {
                            commands.Add(command);
                        }
                        break;
                    default:
                        commands.Add(command);
                        break;
                }
            }
            return commands.ToArray();
        }
    }
}
