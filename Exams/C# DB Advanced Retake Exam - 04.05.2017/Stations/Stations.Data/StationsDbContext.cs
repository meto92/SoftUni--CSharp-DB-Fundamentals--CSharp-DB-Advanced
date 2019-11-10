using Microsoft.EntityFrameworkCore;

using Stations.Data.EntityConfigurations;
using Stations.Models;

namespace Stations.Data
{
    public class StationsDbContext : DbContext
    {
        public StationsDbContext()
        { }

        public StationsDbContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<CustomerCard> CustomerCards { get; set; }

        public DbSet<SeatingClass> SeatingClasses { get; set; }

        public DbSet<Station> Stations { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Train> Trains { get; set; }

        public DbSet<TrainSeats> TrainSeats { get; set; }

        public DbSet<Trip> Trips { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionConfiguration.ConnectionString);

                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            void applyConfig<TEntityTypeConfiguration, TEntity>() 
                where TEntityTypeConfiguration : IEntityTypeConfiguration<TEntity>, new()
                where TEntity : class 
                => modelBuilder.ApplyConfiguration(new TEntityTypeConfiguration());

            applyConfig<CustomerCardConfiguration, CustomerCard>();

            applyConfig<SeatingClassConfiguration, SeatingClass>();

            applyConfig<TicketConfiguration, Ticket>();

            applyConfig<TrainConfiguration, Train>();

            applyConfig<TrainSeatsConfiguration, TrainSeats>();

            applyConfig<TripConfiguration, Trip>();
        }
    }
}