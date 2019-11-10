using System;
using System.Linq;

using CarDealer.Data;
using CarDealer.Models;

namespace CarDealer.DatabaseInitializer
{
    internal class SalesInitializer
    {
        private const int SalesToGenerate = 20;

        private static Sale[] GenerateRandomSales(int[] customerIds, int[] carIds)
        {
            int[] discounts =
            {
                0,
                5,
                10,
                15,
                20,
                30,
                40,
                50
            };
            
            Sale[] randomSales = new Sale[SalesToGenerate];

            Random rnd = new Random();

            for (int i = 0; i < SalesToGenerate; i++)
            {
                int custoerId = customerIds[rnd.Next(customerIds.Length)];

                int carId = carIds[rnd.Next(carIds.Length)];

                int discount = discounts[rnd.Next(discounts.Length)];

                randomSales[i] = new Sale
                {
                    CustomerId = custoerId,
                    CarId = carId,
                    Discount = discount
                };
            }

            return randomSales;
        }

        private static void AddRandomSales(CarDealerContext db)
        {
            int[] customerIds = db.Customers.Select(c => c.Id).ToArray();

            int[] carIds = db.Cars.Select(c => c.Id).ToArray();

            Sale[] randomSales = GenerateRandomSales(customerIds, carIds);

            db.Sales.AddRange(randomSales);

            db.SaveChanges();
        }

        internal static void InitializeSales(CarDealerContext db)
        {
            AddRandomSales(db);
        }
    }
}