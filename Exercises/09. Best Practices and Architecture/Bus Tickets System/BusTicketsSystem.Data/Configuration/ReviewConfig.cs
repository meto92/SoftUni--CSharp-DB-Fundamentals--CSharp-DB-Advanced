using BusTicketsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketsSystem.Data.Configuration
{
    internal class ReviewConfig : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Content)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(r => r.PublishedOn)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(r => r.BusCompany)
                .WithMany(bc => bc.Reviews)
                .HasForeignKey(r => r.BusCompanyId);

            builder.HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerId);
        }
    }
}