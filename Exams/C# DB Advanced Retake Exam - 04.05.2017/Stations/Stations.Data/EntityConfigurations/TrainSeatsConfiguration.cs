using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Stations.Models;

namespace Stations.Data.EntityConfigurations
{
    public class TrainSeatsConfiguration : IEntityTypeConfiguration<TrainSeats>
    {
        public void Configure(EntityTypeBuilder<TrainSeats> builder)
        {
            builder.HasKey(ts => ts.Id);

            builder.HasOne(ts => ts.Train)
                .WithMany(t => t.Seats)
                .HasForeignKey(ts => ts.TrainId);

            builder.HasOne(ts => ts.SeatingClass)
                .WithMany(sc => sc.Seats)
                .HasForeignKey(sc => sc.SeatingClassId);
        }
    }
}