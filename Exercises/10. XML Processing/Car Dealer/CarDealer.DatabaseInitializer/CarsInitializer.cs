using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;

namespace CarDealer.DatabaseInitializer
{
    internal class CarsInitializer
    {
        private const string CarsXmlPath = @"Resources\cars.xml";

        private static CarDto[] ReadCustomers()
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("cars");

            XmlSerializer serializer = new XmlSerializer(typeof(CarDto[]), xmlRoot);

            CarDto[] deserializedCars = null;

            using (StreamReader reader = new StreamReader(CarsXmlPath))
            {
                deserializedCars = (CarDto[]) serializer.Deserialize(reader);
            }

            return deserializedCars;
        }

        private static HashSet<int> GetRandomIds(int[] partIdsArr, int count, Random rnd)
        {
            HashSet<int> ids = new HashSet<int>();

            int partIdsCount = partIdsArr.Length;

            while (ids.Count < count)
            {
                ids.Add(partIdsArr[rnd.Next(partIdsCount)]);
            }

            return ids;
        }

        private static void ImportCars(CarDealerContext db)
        {
            Car[] cars = ReadCustomers()
                .Select(c => new Car
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .ToArray();

            int[] partIdsArr = db.Parts.Select(p => p.Id).ToArray();

            Random rnd = new Random();

            for (int i = 0; i < cars.Length; i++)
            {
                HashSet<int> partIds = GetRandomIds(partIdsArr, rnd.Next(10, 21), rnd);

                PartCar[] carParts = partIds
                    .Select(pId => new PartCar
                    {
                        PartId = pId
                    })
                    .ToArray();

                cars[i].CarParts = carParts;
            }

            db.Cars.AddRange(cars);

            db.SaveChanges();
        }
        
        internal static void InitializeCars(CarDealerContext db)
        {
            ImportCars(db);
        }
    }
}