using Employees.Services;
using System;
using System.Linq;
using System.Reflection;

namespace Employees.App
{
    public class CommandParser
    {
        private const string InvaldCommandMessage = "Invald command name!";

        private EmployeeService employeeService;

        public CommandParser(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public Command ParseCommand(string[] arguments)
        {
            string commandName = arguments[0];
            string[] commandArguments = arguments.Skip(1).ToArray();

            Type commandType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Name == commandName + nameof(Command) &&
                    typeof(Command).IsAssignableFrom(t))
                .FirstOrDefault();

            if (commandType == null)
            {
                throw new NotSupportedException(InvaldCommandMessage);
            }

            Command command = (Command) Activator.CreateInstance(commandType, new object[] { commandArguments });

            FieldInfo employeeServiceField = command.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(fi => fi.FieldType == typeof(EmployeeService))
                .FirstOrDefault();

            employeeServiceField.SetValue(command, this.employeeService);

            return command;
        }
    }
}