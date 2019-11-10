using System.Collections.Generic;

namespace Employees.DTOs
{
    public class ManagerDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<EmployeeDto> Employees { get; set; }

        public int EmployeesCount { get; set; }
    }
}