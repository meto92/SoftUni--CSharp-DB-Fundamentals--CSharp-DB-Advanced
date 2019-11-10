using System.Collections.Generic;

namespace ProductShop.Models
{
    public class User
    {
        public User()
        {
            this.BoughtProducts = new HashSet<Product>();
            this.SoldProducts = new HashSet<Product>();
            this.UserFriends = new HashSet<UserFriend>();
            this.FriendUsers = new HashSet<UserFriend>();
        }

        public int Id { get; private set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => this.FirstName + " " + this.LastName;

        public int? Age { get; set; }

        public virtual ICollection<Product> BoughtProducts { get; set; }

        public virtual ICollection<Product> SoldProducts { get; set; }

        public virtual ICollection<UserFriend> UserFriends { get; set; }

        public virtual ICollection<UserFriend> FriendUsers { get; set; }
    }
}