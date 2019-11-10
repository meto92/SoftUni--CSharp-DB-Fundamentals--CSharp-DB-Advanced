using AutoMapper;
using Employees.Data;
using Employees.DTOs;
using Employees.Models;
using Employees.Services;

namespace Employees.App
{
    public class Startup
    {
        private static void ResetDatabase(EmployeesContext db)
        {
            db.Database.EnsureDeleted();

            db.Database.EnsureCreated();
        }

        public static void Main()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ReverseMap();
                cfg.CreateMap<Employee, EmployeeWithManagerDto>();
                cfg.CreateMap<Employee, ManagerDto>();
            });

            using (EmployeesContext db = new EmployeesContext())
            {
                //ResetDatabase(db);

                EmployeeService employeeService = new EmployeeService(db);
                CommandParser commandParser = new CommandParser(employeeService);

                Engine engine = new Engine(commandParser);

                engine.Run();
            }
        }
    }
}