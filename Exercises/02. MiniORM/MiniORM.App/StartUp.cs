using System.Linq;
using MiniORM.App.Data;
using MiniORM.App.Data.Entities;

namespace MiniORM.App
{
    public class StartUp
    {
        private const string ConnectionString =
            "Server=(LocalDB)\\MSSQLLocalDB;" +
            "Database=MiniORM;" +
            "Integrated Security=true";

        public static void Main()
        {
            SoftUniDbContext db = new SoftUniDbContext(ConnectionString);

            Employee employee = new Employee(
                "Gosho",
                "Inserted",
                db.Departments.First().Id,
                true);

            db.Employees.Add(employee);

            Employee lastEmployee = db.Employees.Last();

            lastEmployee.FirstName = "Modified";

            db.SaveChanges();
        }
    }
}