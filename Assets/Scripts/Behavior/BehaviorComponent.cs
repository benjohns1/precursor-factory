using UnityEngine;

namespace Behavior
{
    abstract class BehaviorComponent : MonoBehaviour
    {
        public abstract string DisplayName { get; }
        public abstract string DisplayType { get;  }
        public CommandType[] CommandTypes;

        public virtual CommandType[] GetCommands(Vector2 mousePosition)
        {
            return CommandTypes;
        }
    }
}
