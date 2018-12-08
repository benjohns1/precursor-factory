using System.Collections.Generic;

namespace CommandSystem
{
    class CommandQueue
    {
        protected Queue<ICommand> queue = new Queue<ICommand>();
    }
}
