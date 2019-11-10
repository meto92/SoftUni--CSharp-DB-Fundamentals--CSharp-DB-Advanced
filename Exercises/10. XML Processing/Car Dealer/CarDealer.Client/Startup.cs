using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealer.Client
{
    public class Startup
    {
        private const string ExportCarsWithDistancePath = @"ExportedData\cars.xml";
        private const string ExportFerrarisPath = @"ExportedData\ferrari-cars.xml";
        private const string ExportLocalSuppliersPath = @"ExportedData\local-suppliers.xml";
        private const string ExportCarsWithPartsPath = @"ExportedData\cars-and-parts.xml";
        private const string ExportCustomersWithTotalSalesPath = @"ExportedData\customers-total-sales.xml";
        private const string ExportSalesWithAppliedDiscountPath = @"ExportedData\sales-discounts.xml";

        private static void ExportXml<T>(T obj, string rootAttributeName, string path)
            where T : class
        {
            XmlRootAttribute root = new XmlRootAttribute(rootAttributeName);
            XmlSerializer serializer = new XmlSerializer(typeof(T), root);
            XmlSerializerNamespaces namespaces =
                new XmlSerializerNamespaces(new[]
                {
                    new XmlQualifiedName(string.Empty, string.Empty)
                });

            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, obj, namespaces);
            }
        }

        // Query 1. Cars with Distance
        private static void ExportCarsWithDistance(CarDealerContext db)
        {
            CarDto[] cars = db.Cars
                .AsNoTracking()
                .ProjectTo<CarDto>()
                .Where(c => c.TravelledDistance > 2e6)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .ToArray();

            ExportXml(cars, "cars", ExportCarsWithDistancePath);
        }

        // Query 2. Cars from make Ferrari 
        private static void ExportFerrariCars(CarDealerContext db)
        {
            FerrariDto[] cars = db.Cars
                .AsNoTracking()
                .Where(c => c.Make == "Ferrari")
                .ProjectTo<FerrariDto>()
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            ExportXml(cars, "cars", ExportFerrarisPath);
        }

        // Query 3. Local Suppliers
        private static void ExportLocalSuppliers(CarDealerContext db)
        {
            LocalSupplierDto[] localSuppliers = db.Suppliers
                .AsNoTracking()
                .Where(s => !s.IsImporter)
                .ProjectTo<LocalSupplierDto>()
                .ToArray();

            ExportXml(localSuppliers, "suppliers", ExportLocalSuppliersPath);
        }
         
        // Query 4. Cars with Their List of Parts
        private static void ExportCarsWithTheirListOfParts(CarDealerContext db)
        {
            CarWithPartsDto[] carsWithParts = db.Cars
                .AsNoTracking()
                .ProjectTo<CarWithPartsDto>()
                .ToArray();

            ExportXml(carsWithParts, "cars", ExportCarsWithPartsPath);
        }

        // Query 5. Total Sales by Customer
        private static void ExportTotalSalesByCustomer(CarDealerContext db)
        {
            CustomerWithTotalSalesDto[] customers = db.Customers
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
                        .Sum(s => (1 - (c.IsYoungDriver ? s.Discount + 5 : s.Discount) / 100.0m) * s.Car.Price)
                })
                .ToArray()
                .Where(c => c.BoughtCars > 1)
                .ToArray();

            ExportXml(customers, "customers", ExportCustomersWithTotalSalesPath);
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

            ExportXml(sales, "sales", ExportSalesWithAppliedDiscountPath);
        }

        public static void Main()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

            CarDealerContext db = new CarDealerContext();

            using (db)
            {
                ExportTotalSalesByCustomer(db);
            }
        }
    }
}