using System.IO;
using System.Linq;
using System.Xml.Serialization;

using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;

namespace CarDealer.DatabaseInitializer
{
    internal class SuppliersInitializer
    {
        private const string SuppliersXmlPath = @"Resources\suppliers.xml";

        private static SupplierDto[] ReadSuppliers()
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("suppliers");

            XmlSerializer serializer = new XmlSerializer(typeof(SupplierDto[]), xmlRoot);

            SupplierDto[] deserializedSuppliers = null;

            using (StreamReader reader = new StreamReader(SuppliersXmlPath))
            {
                deserializedSuppliers = (SupplierDto[]) serializer.Deserialize(reader);
            }

            return deserializedSuppliers;
        }

        private static void ImportSuppliers(CarDealerContext db)
        {
            SupplierDto[] suppliers = ReadSuppliers();

            db.Suppliers.AddRange(suppliers
                .Select(s => new Supplier
                {
                    Name = s.Name,
                    IsImporter = s.IsImporter
                }));

            db.SaveChanges();
        }

        internal static void InitializeSuppliers(CarDealerContext db)
        {
            ImportSuppliers(db);
        }
    }
}