namespace ProductShop.Models
{
    public class UserFriend
    {
        public int UserId { get; set; }

        public int FriendId { get; set; }

        public virtual User User { get; set; }

        public virtual User Friend { get; set; }
    }
}