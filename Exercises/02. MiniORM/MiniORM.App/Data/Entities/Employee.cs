using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniORM.App.Data.Entities
{
    public class Employee
    {
        public Employee()
        { }

        public Employee(string firstName, string lastName, int departmentId, bool isEmployed)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DepartmentId = departmentId;
            this.IsEmployed = IsEmployed;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public bool IsEmployed { get; set; }

        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public ICollection<EmployeeProject> EmployeesProjects { get; }
    }
}