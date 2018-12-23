using UnitTask;

namespace GameEvents.UnitTask
{
    public class UnitTaskStarted : UnitTaskEvent
    {
        public UnitTaskStarted(ITask task) : base(task)
        {
        }
    }
}