using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Data.EntityConfig
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.HasKey(pm => pm.Id);

            builder.Property(pm => pm.Type)
                .HasConversion<string>();

            builder.HasOne(pm => pm.User)
                .WithMany(u => u.PaymentMethods)
                .HasForeignKey(pm => pm.UserId);
            
            builder.HasOne(pm => pm.BankAccount)
                .WithMany(ba => ba.Payments)
                .HasForeignKey(pm => pm.BankAccountId);

            builder.HasOne(pm => pm.CreditCard)
                .WithMany(cc => cc.Payments)
                .HasForeignKey(pm => pm.CreditCardId);
        }
    }
}