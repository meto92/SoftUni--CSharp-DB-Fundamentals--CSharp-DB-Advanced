using System.Collections.Generic;

namespace Instagraph.Models
{
    public class Picture
    {
        public Picture()
        {
            this.Posts = new HashSet<Post>();
        }

        public int Id { get; private set; }

        public string Path { get; set; }

        public decimal Size { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}