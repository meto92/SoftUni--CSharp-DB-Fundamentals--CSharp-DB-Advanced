using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Stations.Models;

namespace Stations.Data.EntityConfigurations
{
    public class CustomerCardConfiguration : IEntityTypeConfiguration<CustomerCard>
    {
        public void Configure(EntityTypeBuilder<CustomerCard> builder)
        {
            builder.HasKey(cc => cc.Id);

            builder.Property(cc => cc.Name)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(cc => cc.Type)
                .HasConversion<string>();
        }
    }
}