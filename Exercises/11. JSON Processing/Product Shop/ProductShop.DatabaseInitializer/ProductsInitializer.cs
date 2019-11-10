using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using ProductShop.Data;
using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop.DatabaseInitializer
{
    internal class ProductsInitializer
    {
        private const string ProductsJsonPath = @"Json\products.json";

        private static ProductDto[] ReadProducts()
        {
            string json = File.ReadAllText(ProductsJsonPath);

            ProductDto[] deserializedProducts = 
                JsonConvert.DeserializeObject<ProductDto[]>(json);

            return deserializedProducts;
        }

        private static void ImportProducts(ProductShopContext db)
        {
            ProductDto[] readProducts = ReadProducts();

            Product[] products = readProducts
                .Select(dto => new Product
                {
                    Name = dto.Name,
                    Price = dto.Price
                })
                .ToArray();

            int[] userIds = db.Users
                .Select(u => u.Id)
                .ToArray();

            int[] categoryIds = db.Categories
                .Select(c => c.Id)
                .ToArray();

            Random rnd = new Random();

            for (int i = 0; i < products.Length; i++)
            {
                products[i].SellerId = userIds[rnd.Next(userIds.Length)];

                if (i % 3 != 0)
                {
                    products[i].BuyerId = userIds[rnd.Next(userIds.Length)];
                }

                int categoriesCount = rnd.Next(1, 5);

                HashSet<int> productCategoryIds = new HashSet<int>();

                for (int j = 0; j < categoriesCount; j++)
                {
                    productCategoryIds.Add(categoryIds[rnd.Next(categoryIds.Length)]);
                }

                productCategoryIds.ToList()
                    .ForEach(categoryId => products[i]
                        .CategoryProducts
                        .Add(new CategoryProduct
                        {
                            CategoryId = categoryId
                        }));
            }

            db.Products.AddRange(products);

            db.SaveChanges();
        }

        internal static void InitializeProducts(ProductShopContext db)
        {
            ImportProducts(db);
        }
    }
}