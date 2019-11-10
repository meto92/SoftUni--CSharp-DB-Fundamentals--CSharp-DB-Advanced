namespace Instagraph.Models
{
    public class Comment
    {
        public int Id { get; private set; }

        public string Content { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }

        public virtual User User { get; set; }

        public virtual Post Post { get; set; }
    }
}