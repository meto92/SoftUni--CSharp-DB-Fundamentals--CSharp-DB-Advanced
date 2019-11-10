using System.IO;
using System.Linq;

using Newtonsoft.Json;

using ProductShop.Data;
using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop.DatabaseInitializer
{
    internal class CategoriesInitializer
    {
        private const string CategoriesJsonPath = @"Json\categories.json";

        private static CategoryDto[] ReadCategories()
        {
            string json = File.ReadAllText(CategoriesJsonPath);

            CategoryDto[] deserializedCategories = 
                JsonConvert.DeserializeObject<CategoryDto[]>(json);

            return deserializedCategories;
        }

        private static void ImportCategories(ProductShopContext db)
        {
            CategoryDto[] categories = ReadCategories();

            db.Categories.AddRange(categories
                .Select(dto => new Category
                {
                    Name = dto.Name
                }));

            db.SaveChanges();
        }

        internal static void InitializeCategories(ProductShopContext db)
        {
            ImportCategories(db);
        }
    }
}