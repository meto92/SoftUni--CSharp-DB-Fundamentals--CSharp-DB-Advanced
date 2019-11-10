using FastFood.Data.EntityConfigurations;
using FastFood.Models;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Data
{
	public class FastFoodDbContext : DbContext
	{
		public FastFoodDbContext()
		{ }

		public FastFoodDbContext(DbContextOptions options)
			: base(options)
		{ }

		protected override void OnConfiguring(DbContextOptionsBuilder builder)
		{
			if (!builder.IsConfigured)
			{
				builder.UseSqlServer(Configuration.ConnectionString);
			}
		}

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
		{
            void applyConfiguration<TEntity, TEntityConfiguration>()
                where TEntity : class
                where TEntityConfiguration : IEntityTypeConfiguration<TEntity>, new()
                => builder.ApplyConfiguration(new TEntityConfiguration());

            applyConfiguration<Category, CategoryConfiguration>();
            applyConfiguration<Employee, EmployeeConfiguration>();
            applyConfiguration<Item, ItemConfiguration>();
            applyConfiguration<Order, OrderConfiguration>();
            applyConfiguration<OrderItem, OrderItemConfiguration>();
            applyConfiguration<Position, PositionConfiguration>();
        }
	}
}