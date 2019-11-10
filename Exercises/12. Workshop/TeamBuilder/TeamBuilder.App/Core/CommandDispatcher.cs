using System;
using System.Linq;
using System.Reflection;

using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.App.Utilities;

namespace TeamBuilder.App.Core
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private const string CommandSuffix = "Command";

        private readonly IServiceProvider serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        private ICommand CreateCommand(string commandName)
        {
            Type commandType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Name.ToUpper() == $"{commandName}{CommandSuffix}".ToUpper() &&
                    typeof(ICommand).IsAssignableFrom(t))
                .FirstOrDefault();

            if (commandType == null)
            {
                throw new NotSupportedException(string.Format(
                    Constants.ErrorMessages.InvalidCommandName,
                    commandName));
            }

            ConstructorInfo constructor = commandType.GetConstructors().First();

            Type[] constructorParameterTypes = constructor.GetParameters()
                .Select(pi => pi.ParameterType)
                .ToArray();

            object[] constructorArguments = constructorParameterTypes
                .Select(serviceProvider.GetService)
                .ToArray();

            ICommand command = (ICommand) Activator.CreateInstance(commandType, constructorArguments);

            return command;
        }

        public string Dispatch(string input)
        {
            string[] arguments = input
                .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            string commandName = arguments.Length > 0 ? arguments[0] : string.Empty;

            ICommand command = CreateCommand(commandName);

            string result = command.Execute(arguments.Skip(1).ToArray());

            return result;
        }
    }
}