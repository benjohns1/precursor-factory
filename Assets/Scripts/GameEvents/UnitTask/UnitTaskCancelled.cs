using UnitTask;

namespace GameEvents.UnitTask
{
    public class UnitTaskCancelled : UnitTaskEvent
    {
        public UnitTaskCancelled(ITask task) : base(task)
        {
        }
    }
}