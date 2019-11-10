using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProductShop.Models;

namespace ProductShop.Data.EntityConfigurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired();

            builder.HasOne(p => p.Seller)
                .WithMany(u => u.SoldProducts)
                .HasForeignKey(p => p.SellerId);

            builder.HasOne(p => p.Buyer)
                .WithMany(u => u.BoughtProducts)
                .HasForeignKey(p => p.BuyerId);
        }
    }
}