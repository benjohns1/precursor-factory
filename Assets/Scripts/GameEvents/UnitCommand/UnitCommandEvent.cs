using UnitCommand;

namespace GameEvents.UnitCommand
{
    public abstract class UnitCommandEvent : IEvent
    {
        public ICommand Command { get; protected set; }

        public UnitCommandEvent(ICommand command)
        {
            Command = command;
        }
    }
}