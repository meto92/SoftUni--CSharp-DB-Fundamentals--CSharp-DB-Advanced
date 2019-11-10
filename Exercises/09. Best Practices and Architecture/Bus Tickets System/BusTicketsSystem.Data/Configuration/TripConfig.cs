using BusTicketsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketsSystem.Data.Configuration
{
    internal class TripConfig : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Status)
                .HasConversion<string>();

            builder.HasOne(t => t.OriginBusStation)
                .WithMany(bs => bs.Departures)
                .HasForeignKey(t => t.OriginBusStationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.DestinationBusStation)
                .WithMany(bs => bs.Arrivals)
                .HasForeignKey(t => t.DestinationBusStationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.BusCompany)
                .WithMany(bc => bc.Trips)
                .HasForeignKey(t => t.BusCompanyId);

            builder.HasMany(trip => trip.Tickets)
                .WithOne(t => t.Trip)
                .HasForeignKey(t => t.TripId);
        }
    }
}