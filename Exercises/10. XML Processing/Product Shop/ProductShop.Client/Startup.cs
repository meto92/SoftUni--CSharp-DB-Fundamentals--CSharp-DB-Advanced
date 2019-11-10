using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using ProductShop.Data;
using ProductShop.DTOs;

namespace ProductShop.Client
{
    public class Startup
    {
        private const string ExportProductsInRangePath = @"ExportedData\products-in-range.xml";
        private const string ExportSoldProductsPath = @"ExportedData\users-sold-products.xml";
        private const string ExportCategoriesByProductsInfoPath = @"ExportedData\categories-by-products.xml";
        private const string ExportUsersAndProductsPath = @"ExportedData\users-and-products.xml";

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

        // Query 1. Products In Range
        private static void ExportProductsInRange(ProductShopContext db, decimal minPrice, decimal maxPrice)
        {
            ProductWithBuyerDto[] products = db.Products
                .Where(p => p.Price >= minPrice &&
                    p.Price <= maxPrice &&
                    p.BuyerId != null)
                .ProjectTo<ProductWithBuyerDto>()
                .OrderBy(p => p.Price)
                .ToArray();

            ExportXml(products, "products", ExportProductsInRangePath);
        }

        // Query 2. Sold Products
        private static void ExportSoldProducts(ProductShopContext db)
        {
            UserSoldProductsDto[] sellers = db.Users
                .ProjectTo<UserSoldProductsDto>()
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToArray()
                .Where(u => u.SoldProducts.Any())
                .ToArray();

            ExportXml(sellers, "users", ExportSoldProductsPath);
        }

        // Query 3. Categories By Products Count
        private static void ExportCategoriesByProductsCount(ProductShopContext db)
        {
            CategoryDto[] categories = db.Categories
                .ProjectTo<CategoryDto>()
                .ToArray();

            ProductDto[] products = db.Products
                .ProjectTo<ProductDto>()
                .ToArray();

            CategoryWithProductsInfoDto[] categoriesWithProductsInfo = categories
                .Select(c => new CategoryWithProductsInfoDto
                {
                    Name = c.Name,
                    ProductsCount = products.Count(p => p.CategoryIds.Contains(c.Id)),
                    AveragePrice = products
                        .Where(p => p.CategoryIds.Contains(c.Id))
                        .Average(p => p?.Price),
                    TotalRevenue = products
                        .Where(p => p.CategoryIds.Contains(c.Id))
                        .Sum(p => p.Price)
                })
                .OrderByDescending(c => c.ProductsCount)
                .ToArray();

            ExportXml(categoriesWithProductsInfo, "categories", ExportCategoriesByProductsInfoPath);
        }

        // Query 4. Users and Products
        private static void ExportUsersAndProducts(ProductShopContext db)
        {
            UserAndProductsDto[] users = db.Users
                .ProjectTo<UserAndProductsDto>()
                .ToArray();

            users = users
                .Where(u => u.SoldProducts.Any())
                .Select(u => new UserAndProductsDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProductsDto = new SoldProductsDto
                    {
                        ProductsCount = u.SoldProducts.Count(),
                        Products = u.SoldProducts
                    }
                })
                .OrderByDescending(u => u.SoldProductsDto.ProductsCount)
                .ThenBy(u => u.LastName)
                .ToArray();

            SellersDto sellers = new SellersDto
            {
                SellersCount = users.Length,
                Sellers = users
            };

            ExportXml(sellers, "users", ExportUsersAndProductsPath);
        }

        public static void Main()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            ProductShopContext db = new ProductShopContext();

            using (db)
            {
                ExportUsersAndProducts(db);
            }
        }
    }
}