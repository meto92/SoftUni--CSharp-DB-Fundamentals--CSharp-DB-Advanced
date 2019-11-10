namespace Employees.App.Commands
{
    public class EmployeeInfoCommand : Command
    {
        public EmployeeInfoCommand(string[] arguments) 
            : base(arguments)
        { }

        public override void Execute()
        {
            int employeeId = int.Parse(base.arguments[0]);

            base.employeeService.PrintEmployeeInfo(employeeId);
        }
    }
}