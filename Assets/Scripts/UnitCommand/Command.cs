namespace UnitCommand
{
    public abstract class Command
    {
        public abstract bool Start(CommandQueueComponent commandQueueComponent);
        public abstract bool Update(CommandQueueComponent commandQueueComponent);
        public virtual void Stop(CommandQueueComponent commandQueueComponent) { }
    }
}
