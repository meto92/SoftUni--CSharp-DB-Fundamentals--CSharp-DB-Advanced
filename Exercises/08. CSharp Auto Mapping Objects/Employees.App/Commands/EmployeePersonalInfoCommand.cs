namespace Employees.App.Commands
{
    public class EmployeePersonalInfoCommand : Command
    {
        public EmployeePersonalInfoCommand(string[] arguments) 
            : base(arguments)
        { }

        public override void Execute()
        {
            int employeeId = int.Parse(base.arguments[0]);

            base.employeeService.PrintEmployeePersonalInfo(employeeId);
        }
    }
}