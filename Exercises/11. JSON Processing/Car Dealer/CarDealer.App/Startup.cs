using System.IO;
using System.Linq;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace CarDealer.App
{
    public class Startup
    {
        private const string ExportOrderedCustomersPath = @"ExportedData\ordered-customers.json";
        private const string ExportToyotaCarsPath = @"ExportedData\toyota-cars.json";
        private const string ExportLocalSuppliersPath = @"ExportedData\local-suppliers.json";
        private const string ExportCarsWithPartsPath = @"ExportedData\cars-and-parts.json";
        private const string ExportCustomersWithTotalSalesPath = @"ExportedData\customers-total-sales.json";
        private const string ExportSalesWithAppliedDiscountPath = @"ExportedData\sales-discounts.json";

        // Query 1. Ordered Customers
        private static void ExportOrderedCustomers(CarDealerContext db)
        {
            Customer[] customers = db.Customers
                .AsNoTracking()
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .ToArray();

            for (int i = 0; i < customers.Length; i++)
            {
                customers[i].Sales = new Sale[0];
            }

            string json = JsonConvert.SerializeObject(customers, Formatting.Indented);

            File.WriteAllText(ExportOrderedCustomersPath, json);
        }

        // Query 2. Cars from make Toyota
        private static void ExportToyotaCars(CarDealerContext db)
        {
            ToyotaDto[] cars = db.Cars
                .AsNoTracking()
                .Where(c => c.Make == "Toyota")
                .ProjectTo<ToyotaDto>()
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            string json = JsonConvert.SerializeObject(cars, Formatting.Indented);

            File.WriteAllText(ExportToyotaCarsPath, json);
        }

        // Query 3. Local Suppliers
        private static void ExportLocalSuppliers(CarDealerContext db)
        {
            LocalSupplierDto[] localSuppliers = db.Suppliers
                .AsNoTracking()
                .Where(s => !s.IsImporter)
                .ProjectTo<LocalSupplierDto>()
                .ToArray();

            string json = JsonConvert.SerializeObject(localSuppliers, Formatting.Indented);

            File.WriteAllText(ExportLocalSuppliersPath, json);
        }

        // Query 4. Cars with Their List of Parts
        private static void ExportCarsWithTheirListOfParts(CarDealerContext db)
        {
            CarWithPartsDto[] carsWithParts = db.Cars
                .AsNoTracking()
                .ProjectTo<CarWithPartsDto>()
                .ToArray();

            string json = JsonConvert.SerializeObject(carsWithParts, Formatting.Indented);

            File.WriteAllText(ExportCarsWithPartsPath, json);
        }

        // Query 5. Total Sales by Customer
        private static void ExportTotalSalesByCustomer(CarDealerContext db)
        {
            CustomerWithTotalSalesDto[] customers = customers = db.Customers
                .Include(c => c.Sales)
                    .ThenInclude(s => s.Car)
                        .ThenInclude(c => c.CarParts)
                            .ThenInclude(cp => cp.Part)
                .ToArray()
                .Select(c => new CustomerWithTotalSalesDto
                {
                    Name = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales
                        .Sum(s => (1 - (decimal)(c.IsYoungDriver ? s.Discount + 0.05 : s.Discount)) * s.Car.Price)
                })
                .ToArray()
                .Where(c => c.BoughtCars > 1)
                .ToArray();

            string json = JsonConvert.SerializeObject(customers, Formatting.Indented);

            File.WriteAllText(ExportCustomersWithTotalSalesPath, json);
        }

        // Query 6. Sales with Applied Discount
        private static void ExportSalesWithAppliedDiscount(CarDealerContext db)
        {
            PartCar[] carsParts = db.PartsCars
                .AsNoTracking()
                .Include(cp => cp.Part)
                .ToArray();

            SaleDto[] sales = db.Sales
                .AsNoTracking()
                .ProjectTo<SaleDto>()
                .ToArray();

            for (int i = 0; i < sales.Length; i++)
            {
                SaleDto sale = sales[i];

                sale.Price = carsParts
                    .Where(cp => cp.CarId == sale.Car.Id)
                    .Sum(cp => cp.Part.Price);

                sale.Discount += sale.IsCustomerYoungDriver ? 0.05 : 0;

                sale.PriceWithDiscount = (1 - (decimal) sale.Discount) * sale.Price;
            }

            string json = JsonConvert.SerializeObject(sales, Formatting.Indented);

            File.WriteAllText(ExportSalesWithAppliedDiscountPath, json);
        }

        public static void Main()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

            CarDealerContext db = new CarDealerContext();

            using (db)
            {
                ExportSalesWithAppliedDiscount(db);
            }
        }
    }
}