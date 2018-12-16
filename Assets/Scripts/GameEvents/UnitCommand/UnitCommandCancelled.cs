using UnitCommand;

namespace GameEvents.UnitCommand
{
    public class UnitCommandCancelled : UnitCommandEvent
    {
        public UnitCommandCancelled(ICommand command) : base(command)
        {
        }
    }
}