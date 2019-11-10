using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PlanetHunters.Models;

namespace PlanetHunters.Data.EntityConfigurations
{
    public class StarSystemConfiguration : IEntityTypeConfiguration<StarSystem>
    {
        public void Configure(EntityTypeBuilder<StarSystem> builder)
        {
            builder.HasKey(ss => ss.Id);

            builder.Property(ss => ss.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasMany(ss => ss.Stars)
                .WithOne(s => s.HostStarSystem)
                .HasForeignKey(s => s.HostStarSystemId);

            builder.HasMany(ss => ss.Planets)
                .WithOne(p => p.HostStarSystem)
                .HasForeignKey(s => s.HostStarSystemId);
        }
    }
}