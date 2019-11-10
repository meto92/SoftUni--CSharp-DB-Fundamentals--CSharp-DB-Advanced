using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PlanetHunters.Models;

namespace PlanetHunters.Data.EntityConfigurations
{
    public class StarConfiguration : IEntityTypeConfiguration<Star>
    {
        public void Configure(EntityTypeBuilder<Star> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasOne(s => s.HostStarSystem)
                .WithMany(ss => ss.Stars)
                .HasForeignKey(s => s.HostStarSystemId);

            builder.HasOne(s => s.Discovery)
                .WithMany(d => d.Stars)
                .HasForeignKey(s => s.DiscoveryId);
        }
    }
}