using BusTicketsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketsSystem.Data.Configuration
{
    internal class BusCompanyConfig : IEntityTypeConfiguration<BusCompany>
    {
        public void Configure(EntityTypeBuilder<BusCompany> builder)
        {
            builder.HasKey(bc => bc.Id);

            builder.HasAlternateKey(bc => bc.Name);

            builder.Property(bc => bc.Nationality)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasMany(bc => bc.Trips)
                .WithOne(t => t.BusCompany)
                .HasForeignKey(t => t.BusCompanyId);

            builder.HasMany(bc => bc.Reviews)
                .WithOne(r => r.BusCompany)
                .HasForeignKey(r => r.BusCompanyId);
        }
    }
}