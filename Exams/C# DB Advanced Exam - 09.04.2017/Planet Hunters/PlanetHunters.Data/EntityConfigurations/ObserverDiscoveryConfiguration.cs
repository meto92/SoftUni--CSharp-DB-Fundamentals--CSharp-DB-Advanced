using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PlanetHunters.Models;

namespace PlanetHunters.Data.EntityConfigurations
{
    public class ObserverDiscoveryConfiguration : IEntityTypeConfiguration<ObserverDiscovery>
    {
        public void Configure(EntityTypeBuilder<ObserverDiscovery> builder)
        {
            builder.HasKey(od => new { od.ObserverId, od.DiscoveryId });

            builder.HasOne(od => od.Observer)
                .WithMany(o => o.ObservationsOfDiscoveries)
                .HasForeignKey(od => od.ObserverId);

            builder.HasOne(od => od.Discovery)
                .WithMany(d => d.Observers)
                .HasForeignKey(od => od.DiscoveryId);
        }
    }
}