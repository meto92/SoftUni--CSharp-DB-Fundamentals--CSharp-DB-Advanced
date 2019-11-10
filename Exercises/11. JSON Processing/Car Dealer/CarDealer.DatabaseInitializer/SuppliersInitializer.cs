using System.IO;
using System.Linq;

using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;

using Newtonsoft.Json;

namespace CarDealer.DatabaseInitializer
{
    internal class SuppliersInitializer
    {
        private const string SuppliersJsonPath = @"Json\suppliers.json";

        private static SupplierDto[] ReadSuppliers()
        {
            string json = File.ReadAllText(SuppliersJsonPath);

            SupplierDto[] deserializedSuppliers = JsonConvert.DeserializeObject<SupplierDto[]>(json);

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