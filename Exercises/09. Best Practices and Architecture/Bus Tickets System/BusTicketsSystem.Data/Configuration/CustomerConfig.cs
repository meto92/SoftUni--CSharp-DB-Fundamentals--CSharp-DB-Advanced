using BusTicketsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketsSystem.Data.Configuration
{
    internal class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Ignore(c => c.FullName);

            builder.Property(c => c.Gender)
                .HasConversion<string>();

            builder.HasOne(c => c.HomeTown)
                .WithMany(t => t.Customers)
                .HasForeignKey(c => c.HomeTownId);

            builder.HasOne(c => c.BankAccount)
                .WithOne(ba => ba.Customer)
                .HasForeignKey<BankAccount>(ba => ba.CustomerId);

            builder.HasMany(c => c.Tickets)
                .WithOne(t => t.Customer)
                .HasForeignKey(t => t.CustomerId);

            builder.HasMany(c => c.Reviews)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId);
        }
    }
}