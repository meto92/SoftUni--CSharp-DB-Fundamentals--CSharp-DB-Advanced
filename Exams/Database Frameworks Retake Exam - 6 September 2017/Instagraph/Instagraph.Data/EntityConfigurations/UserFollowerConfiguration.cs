using Instagraph.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagraph.Data.EntityConfigurations
{
    public class UserFollowerConfiguration : IEntityTypeConfiguration<UserFollower>
    {
        public void Configure(EntityTypeBuilder<UserFollower> builder)
        {
            builder.HasKey(uf => new { uf.UserId, uf.FollowerId });

            //builder.HasOne(uf => uf.User)
            //    .WithMany(u => u.Followers)
            //    .HasForeignKey(uf => uf.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(uf => uf.Follower)
            //    .WithMany(uf => uf.Following)
            //    .HasForeignKey(uf => uf.FollowerId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}