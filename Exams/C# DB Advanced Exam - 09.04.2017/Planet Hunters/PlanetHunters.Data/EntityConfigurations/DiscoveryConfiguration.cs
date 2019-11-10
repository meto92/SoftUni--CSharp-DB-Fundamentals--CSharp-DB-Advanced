using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PlanetHunters.Models;

namespace PlanetHunters.Data.EntityConfigurations
{
    public class DiscoveryConfiguration : IEntityTypeConfiguration<Discovery>
    {
        public void Configure(EntityTypeBuilder<Discovery> builder)
        {
            builder.HasKey(d => d.Id);

            builder.HasOne(d => d.Telescope)
                .WithMany(t => t.Discoveries)
                .HasForeignKey(d => d.TelescopeId);

            builder.HasMany(d => d.Stars)
                .WithOne(s => s.Discovery)
                .HasForeignKey(s => s.DiscoveryId);

            builder.HasMany(d => d.Planets)
                .WithOne(p => p.Discovery)
                .HasForeignKey(p => p.DiscoveryId);
        }
    }
}