using UnityEngine;

namespace UnitCommand.Drill
{
    public class Drill : ICommand
    {
        public CommandQueue CommandQueue { get; protected set; }

        public Drill(CommandQueue commandQueue)
        {
            CommandQueue = commandQueue;
        }
    }
}
