using GameEvents.Actions;

namespace Behavior
{
    [System.Serializable]
    class TaskType
    {
        public InputActionRequested.ActionType Action = InputActionRequested.ActionType.None;
        public string DisplayName = null;
    }
}
