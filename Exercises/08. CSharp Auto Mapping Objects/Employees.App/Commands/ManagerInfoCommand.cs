namespace Employees.App.Commands
{
    public class ManagerInfoCommand : Command
    {
        public ManagerInfoCommand(string[] arguments) 
            : base(arguments)
        { }

        public override void Execute()
        {
            int managerId = int.Parse(base.arguments[0]);

            base.employeeService.PrintManagerInfo(managerId);
        }
    }
}