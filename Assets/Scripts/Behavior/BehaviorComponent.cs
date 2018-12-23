using Cargo;
using UnityEngine;

namespace Behavior
{
    abstract class BehaviorComponent : MonoBehaviour
    {
        public abstract string DisplayName { get; }
        public abstract string DisplayType { get; }
        public TaskType[] TaskTypes = new TaskType[0];

        public virtual TaskType[] GetTasks(Vector2 mousePosition)
        {
            return TaskTypes;
        }

        public abstract int MaxCargoVolume { get; }
        public CargoAmount[] CargoAmounts = new CargoAmount[0];

        public virtual CargoAmount[] GetCargo()
        {
            return CargoAmounts;
        }
    }
}
