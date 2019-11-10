using CarDealer.Data;

using Microsoft.EntityFrameworkCore;

namespace CarDealer.DatabaseInitializer
{
    public class DatabaseInitializer
    {
        public static void InitialSeed(CarDealerContext db)
        {
            SuppliersInitializer.InitializeSuppliers(db);

            PartsInitializer.InitializeParts(db);

            CarsInitializer.InitializeCars(db);

            CustomersInitializer.InitializeCustomers(db);

            SalesInitializer.InitializeSales(db);
        }

        public static void ResetDatabase()
        {
            using (CarDealerContext db = new CarDealerContext())
            {
                db.Database.EnsureDeleted();

                db.Database.Migrate();

                InitialSeed(db);
            }
        }
    }
}