using UnityEngine;

namespace UnitCommand
{
    [RequireComponent(typeof(SelectableComponent))]
    public class CommandQueueComponent : MonoBehaviour
    {
        protected CommandQueue queue;

        private void Awake()
        {
            queue = new CommandQueue(gameObject.GetComponent<SelectableComponent>(), this);
            GameManager.CommandSystem.Register(queue);
        }

        private void OnDestroy()
        {
            GameManager.CommandSystem.Unregister(queue);
        }
    }
}
