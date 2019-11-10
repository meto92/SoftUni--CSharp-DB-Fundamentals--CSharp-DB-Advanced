using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Employees.Data;
using Employees.DTOs;
using Employees.Models;

namespace Employees.Services
{
    public class EmployeeService
    {
        private const string EmployeeNotFoundMessage = "Employee with Id {0} not found!";

        private EmployeesContext db;

        public EmployeeService(EmployeesContext db)
        {
            this.db = db;
        }

        private Employee ById(int employeeId)
        {
            Employee employee = this.db.Employees.Find(employeeId);

            return employee;
        }

        private void ValidateEmployeeNull(Employee employee, int employeeId)
        {
            if (employee == null)
            {
                throw new ArgumentException(string.Format(
                    EmployeeNotFoundMessage, 
                    employeeId));
            }
        }

        public void AddEmployee(EmployeeDto employeeDto)
        {
            Employee employee = Mapper.Map<Employee>(employeeDto);

            this.db.Employees.Add(employee);

            this.db.SaveChanges();
        }

        public void SetBirthDay(int employeeId, DateTime date)
        {
            Employee employee = this.ById(employeeId);

            ValidateEmployeeNull(employee, employeeId);

            employee.BirthDay = date;

            this.db.SaveChanges();
        }

        public void SetAddress(int employeeId, string address)
        {
            Employee employee = this.ById(employeeId);

            ValidateEmployeeNull(employee, employeeId);

            employee.Address = address;

            this.db.SaveChanges();
        }

        public void SetManager(int employeeId, int managerId)
        {
            Employee employee = this.ById(employeeId);

            ValidateEmployeeNull(employee, employeeId);

            Employee manager = this.ById(managerId);

            ValidateEmployeeNull(manager, managerId);

            employee.Manager = manager;

            this.db.SaveChanges();
        }

        public void PrintEmployeeInfo(int employeeId)
        {
            Employee employee = this.ById(employeeId);

            ValidateEmployeeNull(employee, employeeId);

            Console.WriteLine("ID: {0} - {1} {2} - ${3:f2}",
                employee.Id,
                employee.FirstName,
                employee.LastName,
                employee.Salary);
        }

        public void PrintEmployeePersonalInfo(int employeeId)
        {
            Employee employee = this.ById(employeeId);

            ValidateEmployeeNull(employee, employeeId);

            Console.WriteLine("ID: {0} - {1} {2} - ${3:f2}",
                employee.Id,
                employee.FirstName,
                employee.LastName,
                employee.Salary);
            Console.WriteLine("Birthday: {0}",
                employee.BirthDay == null
                    ? "Unknown birthdate"
                    : ((DateTime) employee.BirthDay).ToString("dd-MM-yyyy"));
            Console.WriteLine($"Address: {employee.Address}");
        }

        public void PrintManagerInfo(int managerId)
        {
            Employee manager = this.ById(managerId);

            ValidateEmployeeNull(manager, managerId);

            ManagerDto managerDto = Mapper.Map<ManagerDto>(manager);

            Console.WriteLine("{0} {1} | Employees: {2}",
                managerDto.FirstName,
                managerDto.LastName,
                managerDto.EmployeesCount);

            foreach (EmployeeDto employeeDto in managerDto.Employees)
            {
                Console.WriteLine("    - {0} {1} - ${2:f2}",
                    employeeDto.FirstName,
                    employeeDto.LastName,
                    employeeDto.Salary);
            }
        }

        public void PrintEmployeesOlderThan(int age)
        {
            int currentYear = DateTime.Now.Year;

            EmployeeWithManagerDto[] employeeWithManagerDtos = this.db
                .Employees
                .Where(e => e.BirthDay != null &&
                    currentYear - ((DateTime) e.BirthDay).Year > age)
                .ProjectTo<EmployeeWithManagerDto>()
                .OrderByDescending(e => e.Salary)
                .ToArray();

            foreach (EmployeeWithManagerDto dto in employeeWithManagerDtos)
            {
                string firstName = dto.FirstName;
                string lastName = dto.LastName;
                decimal salary = dto.Salary;
                string managerLastName = dto.ManagerLastName ?? "[no manager]";

                Console.WriteLine("{0} {1} - ${2:f2} - Manager: {3}",
                    firstName,
                    lastName,
                    salary,
                    managerLastName);
            }
        }
    }
}