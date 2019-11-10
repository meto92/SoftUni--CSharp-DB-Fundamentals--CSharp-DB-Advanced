using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Stations.Models;

namespace Stations.Data.EntityConfigurations
{
    public class TrainConfiguration : IEntityTypeConfiguration<Train>
    {
        public void Configure(EntityTypeBuilder<Train> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasIndex(t => t.TrainNumber)
                .IsUnique();

            builder.Property(t => t.TrainNumber)
                .HasMaxLength(10)
                .IsRequired();

            builder.HasMany(t => t.Trips)
                .WithOne(t => t.Train)
                .HasForeignKey(t => t.TrainId);

            builder.HasMany(t => t.Seats)
                .WithOne(s => s.Train)
                .HasForeignKey(s => s.TrainId);
        }
    }
}