﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProductShop.Models;

namespace ProductShop.Data.EntityConfigurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.LastName)
                .IsRequired();

            builder.HasMany(u => u.BoughtProducts)
                .WithOne(p => p.Buyer)
                .HasForeignKey(p => p.BuyerId);

            builder.HasMany(u => u.SoldProducts)
                .WithOne(p => p.Seller)
                .HasForeignKey(p => p.SellerId);
        }
    }
}