using System.IO;
using System.Linq;
using System.Xml.Serialization;

using ProductShop.Data;
using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop.DatabaseInitializer
{
    internal class CategoriesInitializer
    {
        private const string CategoriesXmlPath = @"Resources\categories.xml";

        private static CategoryDto[] ReadCategories()
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("categories");

            XmlSerializer serializer = new XmlSerializer(typeof(CategoryDto[]), xmlRoot);

            CategoryDto[] deserializedCategories = null;

            using (StreamReader reader = new StreamReader(CategoriesXmlPath))
            {
                deserializedCategories = (CategoryDto[]) serializer.Deserialize(reader);
            }

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