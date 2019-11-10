using System.Collections.Generic;

namespace Instagraph.Models
{
    public class Post
    {
        public Post()
        {
            this.Comments = new HashSet<Comment>();
        }

        public int Id { get; private set; }

        public string Caption { get; set; }

        public int UserId { get; set; }

        public int PictureId { get; set; }

        public virtual User User { get; set; }

        public virtual Picture Picture { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}