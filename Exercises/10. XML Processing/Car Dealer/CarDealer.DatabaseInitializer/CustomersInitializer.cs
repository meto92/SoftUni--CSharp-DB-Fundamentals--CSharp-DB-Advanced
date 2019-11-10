using System.IO;
using System.Linq;
using System.Xml.Serialization;

using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;

namespace CarDealer.DatabaseInitializer
{
    internal class CustomersInitializer
    {
        private const string CustomersXmlPath = @"Resources\customers.xml";

        private static CustomerDto[] ReadCustomers()
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("customers");

            XmlSerializer serializer = new XmlSerializer(typeof(CustomerDto[]), xmlRoot);

            CustomerDto[] deserializedCustomers = null;

            using (StreamReader reader = new StreamReader(CustomersXmlPath))
            {
                deserializedCustomers = (CustomerDto[]) serializer.Deserialize(reader);
            }

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