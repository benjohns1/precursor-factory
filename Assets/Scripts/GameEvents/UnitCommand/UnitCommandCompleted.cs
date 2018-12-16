using UnitCommand;

namespace GameEvents.UnitCommand
{
    public class UnitCommandCompleted : UnitCommandEvent
    {
        public UnitCommandCompleted(ICommand command) : base(command)
        {
        }
    }
}