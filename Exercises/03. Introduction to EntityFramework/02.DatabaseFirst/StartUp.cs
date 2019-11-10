using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;
using P02_DatabaseFirst.IO;
using P02_DatabaseFirst.IO.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace P02_DatabaseFirst
{
    public class StartUp
    {
        // 03. Employees Full Information
        private static void PrintEmployeesFullInformation(SoftUniContext db)
        {
            //first, last and middle name, their job title and salary

            List<Employee> employees = db.Employees.ToList();

            StringBuilder result = new StringBuilder();

            foreach (Employee employee in employees)
            {
                string firstName = employee.FirstName;
                string middleName = employee.MiddleName;
                string lastName = employee.LastName;
                string jobTitle = employee.JobTitle;
                string formattedSalary = employee.Salary.ToString("f2");

                result.AppendFormat("{0} {1} {2} {3} {4}",
                    firstName,
                    lastName,
                    middleName,
                    jobTitle,
                    formattedSalary);
                result.AppendLine();
            }

            Console.Write(result);
        }

        // 04. Employees with Salary Over 50 000
        private static void PrintEmployeesWithSalaryOver50000(SoftUniContext db)
        {
            string[] employeeFirstNames = db.Employees
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .Select(e => e.FirstName)
                .ToArray();

            Console.WriteLine(string.Join(Environment.NewLine, employeeFirstNames));
        }

        // 05. Employees from Research and Development
        private static void PrintEmployeesFromResearchAndDevelopment(SoftUniContext db)
        {
            string[] employeesInfo = db.Employees.Include(e => e.Department)
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => $"{e.FirstName} {e.LastName} from {e.Department.Name} - ${e.Salary:f2}")
                .ToArray();

            Console.WriteLine(string.Join(Environment.NewLine, employeesInfo));
        }


        // 06. Adding a New Address and Updating Employee
        private static void AddNewAddressAndUpdatingEmployee(SoftUniContext db)
        {
            Address address = new Address("Vitoshka 15", 4);

            db.Employees.Single(e => e.LastName == "Nakov").Address = address;

            db.SaveChanges();

            db.Employees.Include(e => e.Address)
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address.AddressText)
                .ToList()
                .ForEach(Console.WriteLine);
        }

        // 07. Employees and Projects 
        private static void Print(SoftUniContext db)
        {
            Func<DateTime?, string> formatDate = (date) =>
            {
                return date?.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) ?? "not finished";
            };

            db.Employees.Include(e => e.EmployeesProjects)
                .Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 &&
                    ep.Project.StartDate.Year <= 2003))
                .Include(e => e.Manager)
                .Take(30)
                .Select(e => $"{e.FirstName} {e.LastName} - Manager: {e.Manager.FirstName} {e.Manager.LastName}{Environment.NewLine}" +
                    string.Join(Environment.NewLine, 
                        e.EmployeesProjects
                            .Select(ep => $"--{ep.Project.Name} - {formatDate(ep.Project.StartDate)} - {formatDate(ep.Project.EndDate)}")))
                .ToList()
                .ForEach(Console.WriteLine);
        }

        // 08. Addresses by Town
        private static void PrintAddressesByTown(SoftUniContext db)
        {
            db.Addresses.Include(a => a.Town)
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .Take(10)
                .Select(a => $"{a.AddressText}, {a.Town.Name} - {a.Employees.Count} employees")
                .ToList()
                .ForEach(Console.WriteLine);
        }

        // 09. Employee 147
        private static void Employee147(SoftUniContext db)
        {
            db.Employees.Include(e => e.EmployeesProjects)
                .Where(e => e.EmployeeId == 147)
                .Take(1)
                .Select(e => $"{e.FirstName} {e.LastName} - {e.JobTitle}{Environment.NewLine}" +
                    string.Join(
                        Environment.NewLine, 
                        e.EmployeesProjects
                            .OrderBy(ep => ep.Project.Name)
                            .Select(ep => ep.Project.Name)))
                .ToList()
                .ForEach(Console.WriteLine);
        }

        // 10. Departments with More Than 5 Employees
        private static void PrintDepartmentsWithMoreThan5Employees(SoftUniContext db)
        {
            IWriter writer = new FileWriter();

            string departmentsDelimiter = new string('-', 10);

            db.Departments
                .Where(d => d.Employees.Count > 5)
                .Include(d => d.Manager)
                .Include(d => d.Employees)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .ToArray()
                .Select(d => string.Format("{0} - {1} {2}{3}",
                    d.Name,
                    d.Manager.FirstName,
                    d.Manager.LastName,
                    Environment.NewLine) +
                    string.Join(
                        Environment.NewLine,
                        d.Employees
                            .OrderBy(e => e.FirstName)
                            .ThenBy(e => e.LastName)
                            .Select(e => $"{e.FirstName} {e.LastName} - {e.JobTitle}")))
                .ToList()
                .ForEach(d => writer.AppendLine(d + Environment.NewLine + departmentsDelimiter));

            writer.Flush();
        }


        // 11. Find Latest 10 Projects
        private static void PrintLast10Projects(SoftUniContext db)
        {
            IWriter writer = new FileWriter();

            db.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .Select(p => string.Format("{0}{3}{1}{3}{2}",
                    p.Name,
                    p.Description,
                    p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                    Environment.NewLine))
                .ToList()
                .ForEach(writer.AppendLine);

            writer.Flush();
        }

        // 12. Increase Salaries
        private static void IncreaseSalaries(SoftUniContext db)
        {
            string[] departments =
            {
                "Engineering",
                "Tool Design",
                "Marketing",
                "Information Services"
            };

            List<Employee> employees = db.Employees
                .Include(e => e.Department)
                .Where(e => departments.Contains(e.Department.Name))
                .ToList();

            foreach (var e in employees)
            {
                e.Salary *= 1.12m;
            }

            employees
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => $"{e.FirstName} {e.LastName} (${e.Salary:f2})")
                .ToList()
                .ForEach(Console.WriteLine);

            db.SaveChanges();
        }

        // 13. Find Employees by First Name Starting With "Sa"
        private static void PritEmployeesByFirstNameStartingWithSa(SoftUniContext db)
        {
            db.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => string.Format(
                    "{0} {1} - {2} - (${3:f2})",
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary))
                .ToList()
                .ForEach(Console.WriteLine);
        }

        // 14. Delete Project by Id
        private static void DeleteProjectById(SoftUniContext db)
        {
            int projectId = 2;

            Project projectToDelete = db.Projects.Find(projectId);
            
            if (projectToDelete != null)
            {
                db.EmployeesProjects
                .RemoveRange(db.EmployeesProjects
                    .Where(ep => ep.Project.ProjectId == projectId));

                db.Projects.Remove(projectToDelete);

                db.SaveChanges();
            }

            db.Projects
                .Take(10)
                .Select(p => p.Name)
                .ToList()
                .ForEach(Console.WriteLine);
        }

        // 15. Remove Towns
        private static void RemoveTown(SoftUniContext db, string townName)
        {
            Town townToDelete = db.Towns
                .SingleOrDefault(t => t.Name == townName);

            if (townToDelete == null)
            {
                return;
            }

            int townId = townToDelete.TownId;

            List<Employee> employees = db.Employees
                .Include(e => e.Address)
                .Where(e => e.Address.TownId == townId)
                .ToList();

            foreach (Employee employee in employees)
            {
                employee.AddressId = null;
            }

            List<Address> addressesToDelete = db.Addresses
                .Where(a => a.TownId == townId)
                .ToList();

            db.Addresses.RemoveRange(addressesToDelete);
            db.Towns.Remove(townToDelete);

            Console.WriteLine($"{addressesToDelete.Count} address in {townName} was deleted");
        }

        public static void Main()
        {
            using (SoftUniContext db = new SoftUniContext())
            {
                var entities = db.Departments
                    .Include(d => d.Manager)
                    .Include(d => d.Employees)
                    //.ThenInclude(e => e.EmployeesProjects)
                    .Select(d => 
                        new { DepartmentName = d.Name,
                            Manager = d.Manager.FirstName + " " + d.Manager.LastName,
                            Employees = d.Employees
                                .Select(e => 
                                new { e.FirstName,
                                    e.LastName,
                                    Projects = e.EmployeesProjects
                                        .Select(ep => 
                                             new { ep.Project.Name})
                                             .ToArray()
                                    }),
                            }
                        )
                    .ToList();

                foreach (var item in entities)
                {
                    Console.WriteLine(item.DepartmentName);

                    foreach (var emp in item.Employees)
                    {
                        Console.WriteLine($"--{emp.FirstName} {emp.LastName}");

                        foreach (var project in emp.Projects)
                        {
                            Console.WriteLine($"----{project.Name}");
                        }
                    }
                }
            }
        }
    }
}