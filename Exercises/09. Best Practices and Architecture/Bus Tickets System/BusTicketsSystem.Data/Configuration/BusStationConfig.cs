using BusTicketsSystem.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketsSystem.Data.Configuration
{
    internal class BusStationConfig : IEntityTypeConfiguration<BusStation>
    {
        public void Configure(EntityTypeBuilder<BusStation> builder)
        {
            builder.HasKey(bs => bs.Id);

            builder.HasAlternateKey(bs => bs.Name);

            builder.HasOne(bs => bs.Town)
                .WithMany(t => t.BusStations)
                .HasForeignKey(bs => bs.TownId);

            builder.HasMany(bs => bs.Arrivals)
                .WithOne(t => t.DestinationBusStation)
                .HasForeignKey(t => t.DestinationBusStationId);

            builder.HasMany(bs => bs.Departures)
                .WithOne(t => t.OriginBusStation)
                .HasForeignKey(t => t.OriginBusStationId);
        }
    }
}