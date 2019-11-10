using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PlanetHunters.Models;

namespace PlanetHunters.Data.EntityConfigurations
{
    class AstronomerDiscoveryConfiguration : IEntityTypeConfiguration<AstronomerDiscovery>
    {
        public void Configure(EntityTypeBuilder<AstronomerDiscovery> builder)
        {
            builder.HasKey(ad => new { ad.AstronomerId, ad.DiscoveryId });

            builder.HasOne(ad => ad.Astronomer)
                .WithMany(a => a.PioneeringDiscoveries)
                .HasForeignKey(ad => ad.AstronomerId);

            builder.HasOne(ad => ad.Discovery)
                .WithMany(d => d.Astronomers)
                .HasForeignKey(ad => ad.DiscoveryId);
        }
    }
}