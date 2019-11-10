using Microsoft.EntityFrameworkCore;

using ProductShop.Data;

namespace ProductShop.DatabaseInitializer
{
    public class DatabaseInitializer
    {
        public static void InitialSeed(ProductShopContext db)
        {
            UsersInitializer.InitializeUsers(db);

            CategoriesInitializer.InitializeCategories(db);

            ProductsInitializer.InitializeProducts(db);
        }

        public static void ResetDatabase()
        {
            using (ProductShopContext db = new ProductShopContext())
            {
                db.Database.EnsureDeleted();

                db.Database.Migrate();

                InitialSeed(db);
            }
        }
    }
}