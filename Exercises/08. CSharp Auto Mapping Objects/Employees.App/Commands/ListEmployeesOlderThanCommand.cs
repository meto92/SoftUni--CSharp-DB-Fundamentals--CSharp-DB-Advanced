namespace Employees.App.Commands
{
    public class ListEmployeesOlderThanCommand : Command
    {
        public ListEmployeesOlderThanCommand(string[] arguments) 
            : base(arguments)
        { }

        public override void Execute()
        {
            int age = int.Parse(base.arguments[0]);

            base.employeeService.PrintEmployeesOlderThan(age);
        }
    }
}