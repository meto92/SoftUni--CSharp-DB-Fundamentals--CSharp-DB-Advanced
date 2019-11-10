using System;
using System.IO;
using System.Linq;

using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;

using Newtonsoft.Json;

namespace CarDealer.DatabaseInitializer
{
    internal class PartsInitializer
    {
        private const string PartsJsonPath = @"Json\parts.json";

        private static PartDto[] ReadParts()
        {
            string json = File.ReadAllText(PartsJsonPath);
            
            PartDto[] deserializedParts = JsonConvert.DeserializeObject<PartDto[]>(json);

            return deserializedParts;
        }

        private static void ImportParts(CarDealerContext db)
        {
            PartDto[] parts = ReadParts();

            int[] supplierIds = db.Suppliers.Select(s => s.Id).ToArray();

            Random rnd = new Random();

            db.Parts.AddRange(parts
                .Select(p => new Part
                {
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SupplierId = supplierIds[rnd.Next(supplierIds.Length)]
                }));

            db.SaveChanges();
        }

        internal static void InitializeParts(CarDealerContext db)
        {
            ImportParts(db);
        }
    }
}