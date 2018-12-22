using GameEvents.Actions;

namespace Behavior
{
    [System.Serializable]
    class CommandType
    {
        public InputActionRequested.ActionType Action;
        public string DisplayName;
    }
}
