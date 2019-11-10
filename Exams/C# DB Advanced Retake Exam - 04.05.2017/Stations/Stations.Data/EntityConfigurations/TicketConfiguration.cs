using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Stations.Models;

namespace Stations.Data.EntityConfigurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.SeatingPlace)
                .HasMaxLength(8)
                .IsRequired();

            builder.HasOne(t => t.Trip)
                .WithMany(t => t.Tickets)
                .HasForeignKey(t => t.TripId);

            builder.HasOne(t => t.PersonalCard)
                .WithMany(pc => pc.Tickets)
                .HasForeignKey(t => t.PersonalCardId);
        }
    }
}