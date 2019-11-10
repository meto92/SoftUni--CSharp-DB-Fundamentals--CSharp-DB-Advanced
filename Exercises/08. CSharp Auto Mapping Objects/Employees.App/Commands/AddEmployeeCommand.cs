using Employees.DTOs;

namespace Employees.App.Commands
{
    public class AddEmployeeCommand : Command
    {
        public AddEmployeeCommand(string[] arguments) 
            : base(arguments)
        { }

        public override void Execute()
        {
            string firstName = base.arguments[0];
            string lastName = base.arguments[1];
            decimal salary = decimal.Parse(base.arguments[2]);

            EmployeeDto employeeDto = new EmployeeDto(firstName, lastName, salary);

            base.employeeService.AddEmployee(employeeDto);
        }
    }
}