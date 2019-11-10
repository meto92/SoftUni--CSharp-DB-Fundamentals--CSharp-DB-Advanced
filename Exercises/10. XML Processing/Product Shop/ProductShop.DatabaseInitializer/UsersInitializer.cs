using System.IO;
using System.Linq;
using System.Xml.Serialization;

using ProductShop.Data;
using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop.DatabaseInitializer
{
    internal class UsersInitializer
    {
        private const string UsersXmlPath = @"Resources\users.xml";

        private static UserDto[] ReadUsers()
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("users");

            XmlSerializer serializer = new XmlSerializer(typeof(UserDto[]), xmlRoot);

            UserDto[] deserializedUsers = null;

            using (StreamReader reader = new StreamReader(UsersXmlPath))
            {
                deserializedUsers = (UserDto[]) serializer.Deserialize(reader);
            }

            return deserializedUsers;
        }

        private static void ImportUsers(ProductShopContext db)
        {
            UserDto[] users = ReadUsers();

            db.AddRange(users
                .Select(dto => new User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Age = dto.Age == 0 ? null : (int?) dto.Age
                }));

            db.SaveChanges();
        }

        internal static void InitializeUsers(ProductShopContext db)
        {
            ImportUsers(db);
        }
    }
}