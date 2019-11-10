﻿namespace PhotoShare.Client.Core
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Client.Core.Contracts;

    public class CommandInterpreter : ICommandInterpreter
    {
        private const string InvalidCommandMessage = "Command {0} not valid!";

        private readonly IServiceProvider serviceProvider;

        public CommandInterpreter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public string Read(string[] input)
        {
            string inputCommand = input[0] + "Command";

            string[] args = input.Skip(1).ToArray();

            var type = Assembly.GetCallingAssembly()
                               .GetTypes()
                               .FirstOrDefault(x => x.Name == inputCommand &&
                                    typeof(ICommand).IsAssignableFrom(x));

            if (type == null)
            {
                throw new InvalidOperationException(string.Format(
                    InvalidCommandMessage,
                    input[0]));
            }

            var constructor = type.GetConstructors()
                                  .First();

            var constructorParameters = constructor.GetParameters()
                                                   .Select(x => x.ParameterType)
                                                   .ToArray();

            var service = constructorParameters.Select(serviceProvider.GetService)
                                               .ToArray();

            var command = (ICommand)constructor.Invoke(service);

            if (args.Length < command.RequiredArgumentsCount)
            {
                throw new InvalidOperationException(string.Format(
                    InvalidCommandMessage,
                    input[0]));
            }

            string result = command.Execute(args);

            return result;
        }
    }
}