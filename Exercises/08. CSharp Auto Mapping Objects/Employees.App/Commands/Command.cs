using Employees.Services;

namespace Employees.App
{
    public abstract class Command
    {
        protected readonly string[] arguments;
        protected EmployeeService employeeService;

        protected Command(string[] arguments)
        {
            this.arguments = arguments;
        }

        public abstract void Execute();
    }
}