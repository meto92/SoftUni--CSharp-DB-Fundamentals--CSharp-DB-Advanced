using CarDealer.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealer.Data.EntityConfigurations
{
    public class PartCarConfiguration : IEntityTypeConfiguration<PartCar>
    {
        public void Configure(EntityTypeBuilder<PartCar> builder)
        {
            builder.HasKey(pc => new { pc.PartId, pc.CarId });

            builder.HasOne(pc => pc.Part)
                .WithMany(p => p.PartCars)
                .HasForeignKey(pc => pc.PartId);

            builder.HasOne(pc => pc.Car)
                .WithMany(c => c.CarParts)
                .HasForeignKey(pc => pc.CarId);
        }
    }
}