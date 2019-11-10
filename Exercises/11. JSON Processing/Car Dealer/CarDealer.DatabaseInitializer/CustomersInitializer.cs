using System.IO;
using System.Linq;

using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;

using Newtonsoft.Json;

namespace CarDealer.DatabaseInitializer
{
    internal class CustomersInitializer
    {
        private const string CustomersJsonPath = @"Json\customers.json";

        private static CustomerDto[] ReadCustomers()
        {
            string json = File.ReadAllText(CustomersJsonPath);

            CustomerDto[] deserializedCustomers = JsonConvert.DeserializeObject<CustomerDto[]>(json);

            return deserializedCustomers;
        }

        private static void ImportCustomers(CarDealerContext db)
        {
            CustomerDto[] customers = ReadCustomers();

            db.Customers.AddRange(customers
                .Select(c => new Customer
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    IsYoungDriver = c.IsYoungDriver
                }));

            db.SaveChanges();
        }

        internal static void InitializeCustomers(CarDealerContext db)
        {
            ImportCustomers(db);
        }
    }
}