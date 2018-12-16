using UnitCommand;

namespace GameEvents.UnitCommand
{
    public class UnitCommandStarted : UnitCommandEvent
    {
        public UnitCommandStarted(ICommand command) : base(command)
        {
        }
    }
}