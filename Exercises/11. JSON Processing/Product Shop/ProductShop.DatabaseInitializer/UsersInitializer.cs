using System.IO;
using System.Linq;

using Newtonsoft.Json;

using ProductShop.Data;
using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop.DatabaseInitializer
{
    internal class UsersInitializer
    {
        private const string UsersJsonPath = @"Json\users.json";

        private static UserDto[] ReadUsers()
        {
            string json = File.ReadAllText(UsersJsonPath);

            UserDto[] deserializedUsers = 
                JsonConvert.DeserializeObject<UserDto[]>(json);

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
                    Age = dto.Age
                }));

            db.SaveChanges();
        }

        internal static void InitializeUsers(ProductShopContext db)
        {
            ImportUsers(db);
        }
    }
}