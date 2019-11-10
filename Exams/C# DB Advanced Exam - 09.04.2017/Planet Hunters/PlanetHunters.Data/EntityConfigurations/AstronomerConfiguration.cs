using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PlanetHunters.Models;

namespace PlanetHunters.Data.EntityConfigurations
{
    public class AstronomerConfiguration : IEntityTypeConfiguration<Astronomer>
    {
        public void Configure(EntityTypeBuilder<Astronomer> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(a => a.LastName)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}