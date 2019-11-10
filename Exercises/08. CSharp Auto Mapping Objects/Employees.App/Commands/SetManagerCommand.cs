namespace Employees.App.Commands
{
    public class SetManagerCommand : Command
    {
        public SetManagerCommand(string[] arguments) 
            : base(arguments)
        { }

        public override void Execute()
        {
            int employeeId = int.Parse(base.arguments[0]);
            int managerId = int.Parse(base.arguments[1]);

            base.employeeService.SetManager(employeeId, managerId);
        }
    }
}