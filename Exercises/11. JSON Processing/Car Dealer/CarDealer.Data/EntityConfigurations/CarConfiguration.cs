using CarDealer.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealer.Data.EntityConfigurations
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Make)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.Model)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(c => c.Sale)
                .WithOne(s => s.Car)
                .HasForeignKey<Sale>(s => s.CarId);
        }
    }
}