using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PlanetHunters.Models;

namespace PlanetHunters.Data.EntityConfigurations
{
    public class PlanetConfiguration : IEntityTypeConfiguration<Planet>
    {
        public void Configure(EntityTypeBuilder<Planet> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.HostStarSystem)
                .WithMany(ss => ss.Planets)
                .HasForeignKey(p => p.HostStarSystemId);

            builder.HasOne(p => p.Discovery)
                .WithMany(d => d.Planets)
                .HasForeignKey(p => p.DiscoveryId);
        }
    }
}