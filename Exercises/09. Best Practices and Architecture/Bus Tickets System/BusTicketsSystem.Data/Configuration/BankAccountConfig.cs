using BusTicketsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketsSystem.Data.Configuration
{
    internal class BankAccountConfig : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.HasKey(ba => ba.Id);

            builder.HasAlternateKey(ba => ba.AccountNumber);

            builder.HasOne(ba => ba.Customer)
                .WithOne(c => c.BankAccount)
                .HasForeignKey<BankAccount>(ba => ba.CustomerId);
        }
    }
}