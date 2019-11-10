using System;

using BusTicketsSystem.Client.Core.Interfaces;

namespace BusTicketsSystem.Client.Core.Commands
{
    public class ExitCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Environment.Exit(0);

            return null;
        }
    }
}