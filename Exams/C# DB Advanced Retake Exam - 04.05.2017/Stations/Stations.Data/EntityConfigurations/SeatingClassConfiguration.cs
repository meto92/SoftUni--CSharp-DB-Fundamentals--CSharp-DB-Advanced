using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Stations.Models;

namespace Stations.Data.EntityConfigurations
{
    public class SeatingClassConfiguration : IEntityTypeConfiguration<SeatingClass>
    {
        public void Configure(EntityTypeBuilder<SeatingClass> builder)
        {
            builder.HasKey(sc => sc.Id);

            builder.HasIndex(sc => sc.Name)
                .IsUnique();

            builder.Property(sc => sc.Name)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(sc => sc.Abbreviation)
                .HasMaxLength(2)
                .IsRequired();

            builder.HasMany(sc => sc.Seats)
                .WithOne(s => s.SeatingClass)
                .HasForeignKey(s => s.SeatingClassId);
        }
    }
}