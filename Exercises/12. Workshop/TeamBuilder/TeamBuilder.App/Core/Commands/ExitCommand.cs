using System;

using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.App.Utilities;

namespace TeamBuilder.App.Core.Commands
{
    public class ExitCommand : ICommand
    {
        public string Execute(string[] arguments)
        {
            Console.WriteLine(Constants.SuccessMessages.Bye);

            Environment.Exit(0);

            return null;
        }
    }
}