using BusTicketsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketsSystem.Data.Configuration
{
    internal class TownConfig : IEntityTypeConfiguration<Town>
    {
        public void Configure(EntityTypeBuilder<Town> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasMaxLength(50);

            builder.HasAlternateKey(t => t.Name);

            builder.HasMany(t => t.Customers)
                .WithOne(c => c.HomeTown)
                .HasForeignKey(c => c.HomeTownId);

            builder.HasMany(t => t.BusStations)
                .WithOne(bs => bs.Town)
                .HasForeignKey(bs => bs.TownId);
        }
    }
}