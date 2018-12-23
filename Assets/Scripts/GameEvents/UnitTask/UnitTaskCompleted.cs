using UnitTask;

namespace GameEvents.UnitTask
{
    public class UnitTaskCompleted : UnitTaskEvent
    {
        public UnitTaskCompleted(ITask task) : base(task)
        {
        }
    }
}