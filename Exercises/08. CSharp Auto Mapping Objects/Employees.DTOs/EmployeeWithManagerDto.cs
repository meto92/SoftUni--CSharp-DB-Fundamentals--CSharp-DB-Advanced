namespace Employees.DTOs
{
    public class EmployeeWithManagerDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public string ManagerLastName { get; set; }
    }
}