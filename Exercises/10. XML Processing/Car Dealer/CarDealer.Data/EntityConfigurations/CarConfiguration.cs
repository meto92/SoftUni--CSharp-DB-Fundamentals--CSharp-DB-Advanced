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
                .IsRequired();

            builder.Property(c => c.Model)
                .IsRequired();

            builder.HasOne(c => c.Sale)
                .WithOne(s => s.Car)
                .HasForeignKey<Sale>(s => s.CarId);
        }
    }
}