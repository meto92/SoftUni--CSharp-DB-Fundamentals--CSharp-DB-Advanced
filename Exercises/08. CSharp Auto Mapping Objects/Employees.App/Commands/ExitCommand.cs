using System;

namespace Employees.App.Commands
{
    public class ExitCommand : Command
    {
        public ExitCommand(string[] arguments) 
            : base(arguments)
        { }

        public override void Execute()
        {
            Environment.Exit(0);
        }
    }
}