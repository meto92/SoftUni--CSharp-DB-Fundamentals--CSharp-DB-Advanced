namespace ProductShop.App
{
    using System.IO;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

    using Newtonsoft.Json;

    using ProductShop.DTOs;

    public class StartUp
    {
        private const string ExportProductsInRangePath = @"ExportedData\products-in-range.json";
        private const string ExportUsersSoldProductsPath = @"ExportedData\users-sold-products.json";
        private const string ExportCategoriesByProductsInfoPath = @"ExportedData\categories-by-products.json";
        private const string ExportUsersAndProductsPath = @"ExportedData\users-and-products.json";
        
        // Query 1. Products In Range
        private static void ExportProductsInRange(ProductShopContext db, decimal minPrice, decimal maxPrice)
        {
            ProductWithSellerDto[] products = db.Products
                .Where(p => p.Price >= minPrice &&
                    p.Price <= maxPrice)
                .ProjectTo<ProductWithSellerDto>()
                .OrderBy(p => p.Price)
                .ToArray();

            string json = JsonConvert.SerializeObject(products, Formatting.Indented);

            File.WriteAllText(ExportProductsInRangePath, json);
        }

        // Query 2. Sold Products
        private static void ExportSoldProducts(ProductShopContext db)
        {
            UserSoldProductsDto[] sellers = db.Users
                .ProjectTo<UserSoldProductsDto>()
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToArray()
                .Where(u => u.ProductsSold.Any() &&
                    u.ProductsSold.Any(p => p.BuyerId != null))
                .ToArray();

            string json = JsonConvert.SerializeObject(sellers, Formatting.Indented);

            File.WriteAllText(ExportUsersSoldProductsPath, json);
        }

        // Query 3. Categories By Products Count
        private static void ExportCategoriesByProductsCount(ProductShopContext db)
        {
            CategoryDto[] categories = db.Categories
                .ProjectTo<CategoryDto>()
                .ToArray();

            ProductWithCategoryIdsDto[] products = db.Products
                .ProjectTo<ProductWithCategoryIdsDto>()
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

            string json = JsonConvert.SerializeObject(categoriesWithProductsInfo, Formatting.Indented);

            File.WriteAllText(ExportCategoriesByProductsInfoPath, json);
        }

        // Query 4. Users and Products
        private static void ExportUsersAndProducts(ProductShopContext db)
        {
            UserSoldProductsDto[] users = db.Users
                .ProjectTo<UserSoldProductsDto>()
                .ToArray();

            UserAndProductsDto[] sellers = users
                .Where(u => u.ProductsSold.Any())
                .Select(u => new UserAndProductsDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProductsDto = new SoldProductsDto
                    {
                        ProductsCount = u.ProductsSold.Length,
                        Products = u.ProductsSold
                            .Select(p => new ProductDto
                            {
                                Name = p.Name,
                                Price = p.Price
                            }).ToArray()
                    }
                })
                .OrderByDescending(u => u.SoldProductsDto.ProductsCount)
                .ThenBy(u => u.LastName)
                .ToArray();

            UsersAndProductsDto obj = new UsersAndProductsDto
            {
                Users = sellers,
                UsersCount = sellers.Length
            };

            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);

            File.WriteAllText(ExportUsersAndProductsPath, json);
        }

        public static void Main()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            ProductShopContext context = new ProductShopContext();

            using (context)
            {
                ExportCategoriesByProductsCount(context);
            }
        }
    }
}