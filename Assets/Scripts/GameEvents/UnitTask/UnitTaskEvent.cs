using UnitTask;

namespace GameEvents.UnitTask
{
    public abstract class UnitTaskEvent : IEvent
    {
        public ITask Task { get; protected set; }

        public UnitTaskEvent(ITask task)
        {
            Task = task;
        }
    }
}