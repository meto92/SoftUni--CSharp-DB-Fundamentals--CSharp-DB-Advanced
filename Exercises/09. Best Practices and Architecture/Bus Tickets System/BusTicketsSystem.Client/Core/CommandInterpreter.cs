using System.Linq;

using BusTicketsSystem.Client.Core.Interfaces;

namespace BusTicketsSystem.Client.Core
{
    public class CommandInterpreter : ICommandInterpreter
    {
        private readonly ICommandParser commandParser;

        public CommandInterpreter(ICommandParser commandParser)
        {
            this.commandParser = commandParser;
        }

        public string InterpretCommand(string[] input)
        {
            string commandName = input[0].Replace("-", string.Empty).ToUpper();
            string[] arguments = input.Skip(1).ToArray();

            ICommand command = this.commandParser.ParseCommand(commandName);

            string result = command.Execute(arguments);

            return result;
        }
    }
}