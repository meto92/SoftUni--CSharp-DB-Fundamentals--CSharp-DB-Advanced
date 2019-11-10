using System;
using System.Linq;
using System.Reflection;

using BusTicketsSystem.Client.Core.Interfaces;

namespace BusTicketsSystem.Client.Core
{
    public class CommandParser : ICommandParser
    {
        private const string CommandSuffix = "Command";
        private const string InvalidCommandMessage = "Invalid command name";

        private readonly IServiceProvider serviceProvider;

        public CommandParser(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ICommand ParseCommand(string commandName)
        {
            Type commandType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Name.ToUpper() == $"{commandName}{CommandSuffix}".ToUpper() &&
                    typeof(ICommand).IsAssignableFrom(t))
                .FirstOrDefault();

            if (commandType == null)
            {
                throw new ArgumentException(InvalidCommandMessage);
            }

            ConstructorInfo ctor = commandType.GetConstructors().First();

            Type[] ctorParameters = ctor.GetParameters()
                .Select(p => p.ParameterType)
                .ToArray();

            object[] services = ctorParameters
                .Select(this.serviceProvider.GetService)
                .ToArray();

            ICommand command = (ICommand) ctor.Invoke(services);

            return command;
        }
    }
}