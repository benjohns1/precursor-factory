using UnityEngine;

namespace UnitTask
{
    [RequireComponent(typeof(SelectableComponent))]
    public class TaskQueueComponent : MonoBehaviour
    {
        protected TaskQueue queue;

        private void Awake()
        {
            queue = new TaskQueue(gameObject.GetComponent<SelectableComponent>(), this);
            GameManager.TaskSystem.Register(queue);
        }

        private void OnDestroy()
        {
            GameManager.TaskSystem.Unregister(queue);
        }
    }
}
