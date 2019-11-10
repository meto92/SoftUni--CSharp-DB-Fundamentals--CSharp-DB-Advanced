using System;
using System.Globalization;

namespace Employees.App.Commands
{
    public class SetBirthdayCommand : Command
    {
        public SetBirthdayCommand(string[] arguments) 
            : base(arguments)
        { }

        public override void Execute()
        {
            int employeeId = int.Parse(base.arguments[0]);
            DateTime date = DateTime.ParseExact(base.arguments[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);

            base.employeeService.SetBirthDay(employeeId, date);
        }
    }
}