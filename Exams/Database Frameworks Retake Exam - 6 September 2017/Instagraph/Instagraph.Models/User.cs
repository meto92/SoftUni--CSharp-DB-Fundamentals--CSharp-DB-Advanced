using System.Collections.Generic;

namespace Instagraph.Models
{
    public class User
    {
        public User()
        {
            this.Followers = new HashSet<User>();

            this.Following = new HashSet<User>();

            this.Comments = new HashSet<Comment>();

            this.Posts = new HashSet<Post>();
        }

        public int Id { get; private set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int ProfilePictureId { get; set; }

        public virtual Picture ProfilePicture { get; set; }

        public virtual ICollection<User> Followers { get; set; }
               
        public virtual ICollection<User> Following { get; set; }
               
        public virtual ICollection<Comment> Comments { get; set; }
               
        public virtual ICollection<Post> Posts { get; set; }
    }
}