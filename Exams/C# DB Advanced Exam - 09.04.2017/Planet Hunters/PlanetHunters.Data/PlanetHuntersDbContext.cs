using Microsoft.EntityFrameworkCore;
using PlanetHunters.Data.EntityConfigurations;
using PlanetHunters.Models;

namespace PlanetHunters.Data
{
    public class PlanetHuntersDbContext : DbContext
    {
        public PlanetHuntersDbContext()
        { }

        public PlanetHuntersDbContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Astronomer> Astronomers { get; set; }

        public DbSet<AstronomerDiscovery> AstronomersDiscoveries { get; set; }

        public DbSet<Discovery> Discoveries { get; set; }

        public DbSet<ObserverDiscovery> ObserversDiscoveries { get; set; }

        public DbSet<Planet> Planets { get; set; }

        public DbSet<Star> Stars { get; set; }

        public DbSet<StarSystem> StarSystems { get; set; }

        public DbSet<Telescope> Telescopes { get; set; }

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
            modelBuilder.ApplyConfiguration(new AstronomerConfiguration());

            modelBuilder.ApplyConfiguration(new AstronomerDiscoveryConfiguration());

            modelBuilder.ApplyConfiguration(new DiscoveryConfiguration());

            modelBuilder.ApplyConfiguration(new ObserverDiscoveryConfiguration());

            modelBuilder.ApplyConfiguration(new PlanetConfiguration());

            modelBuilder.ApplyConfiguration(new StarConfiguration());

            modelBuilder.ApplyConfiguration(new StarSystemConfiguration());

            modelBuilder.ApplyConfiguration(new AstronomerConfiguration());
        }
    }
}