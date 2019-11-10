namespace Employees.App.Commands
{
    public class SetAddressCommand : Command
    {
        public SetAddressCommand(string[] arguments)
            : base(arguments)
        { }

        public override void Execute()
        {
            int employeeId = int.Parse(base.arguments[0]);
            string address = base.arguments[1];

            base.employeeService.SetAddress(employeeId, address);
        }
    }
}