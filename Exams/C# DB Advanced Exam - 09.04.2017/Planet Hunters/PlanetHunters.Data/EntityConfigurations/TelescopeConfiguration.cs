using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PlanetHunters.Models;

namespace PlanetHunters.Data.EntityConfigurations
{
    public class TelescopeConfiguration : IEntityTypeConfiguration<Telescope>
    {
        public void Configure(EntityTypeBuilder<Telescope> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(t => t.Location)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}