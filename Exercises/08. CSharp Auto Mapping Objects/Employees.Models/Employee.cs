using System;
using System.Collections.Generic;

namespace Employees.Models
{
    public class Employee
    {
        public Employee()
        {
            this.Employees = new List<Employee>();
        }

        public int Id { get; private set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public DateTime? BirthDay { get; set; }

        public string Address { get; set; }

        public int? ManagerId { get; set; }

        public Employee Manager { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}