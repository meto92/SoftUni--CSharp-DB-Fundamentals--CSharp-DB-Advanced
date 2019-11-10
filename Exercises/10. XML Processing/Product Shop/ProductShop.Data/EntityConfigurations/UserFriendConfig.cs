using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProductShop.Models;

namespace ProductShop.Data.EntityConfigurations
{
    public class UserFriendConfig : IEntityTypeConfiguration<UserFriend>
    {
        public void Configure(EntityTypeBuilder<UserFriend> builder)
        {
            builder.HasKey(uf => new { uf.UserId, uf.FriendId });

            builder.HasOne(uf => uf.User)
                .WithMany(u => u.UserFriends)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(uf => uf.Friend)
                .WithMany(u => u.FriendUsers)
                .HasForeignKey(uf => uf.FriendId);
        }
    }
}