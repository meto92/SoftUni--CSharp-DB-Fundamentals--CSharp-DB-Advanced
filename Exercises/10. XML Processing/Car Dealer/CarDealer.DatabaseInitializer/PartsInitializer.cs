﻿using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;

namespace CarDealer.DatabaseInitializer
{
    internal class PartsInitializer
    {
        private const string PartsXmlPath = @"Resources\parts.xml";

        private static PartDto[] ReadParts()
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("parts");

            XmlSerializer serializer = new XmlSerializer(typeof(PartDto[]), xmlRoot);

            PartDto[] deserializedParts = null;

            using (StreamReader reader = new StreamReader(PartsXmlPath))
            {
                deserializedParts = (PartDto[]) serializer.Deserialize(reader);
            }

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