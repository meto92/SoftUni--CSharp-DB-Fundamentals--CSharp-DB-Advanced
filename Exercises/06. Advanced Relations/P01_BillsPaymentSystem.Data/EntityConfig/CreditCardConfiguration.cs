using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Data.EntityConfig
{
    public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder.HasKey(cc => cc.CreditCardId);

            builder.Ignore(cc => cc.LimitLeft);

            builder.Property(cc => cc.ExpirationDate)
                .HasColumnType("DATE");

            builder.HasMany(cc => cc.Payments)
                .WithOne(pm => pm.CreditCard)
                .HasForeignKey(pm => pm.CreditCardId);
        }
    }
}