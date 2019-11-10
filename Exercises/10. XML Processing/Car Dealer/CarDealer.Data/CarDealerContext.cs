using CarDealer.Data.EntityConfigurations;
using CarDealer.Models;

using Microsoft.EntityFrameworkCore;

namespace CarDealer.Data
{
    public class CarDealerContext : DbContext
    {
        public CarDealerContext()
        { }

        public CarDealerContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Part> Parts { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<PartCar> PartsCars { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnextionString);

                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CarConfiguration());

            modelBuilder.ApplyConfiguration(new CustomerConfiguration());

            modelBuilder.ApplyConfiguration(new PartConfiguration());

            modelBuilder.ApplyConfiguration(new PartCarConfiguration());

            modelBuilder.ApplyConfiguration(new SaleConfiguration());

            modelBuilder.ApplyConfiguration(new SupplierConfiguration());
        }
    }
}