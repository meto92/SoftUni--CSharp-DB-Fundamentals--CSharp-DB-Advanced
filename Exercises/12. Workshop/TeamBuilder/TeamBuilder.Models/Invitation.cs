namespace TeamBuilder.Models
{
    public class Invitation
    {
        public Invitation()
        {
            this.IsActive = true;
        }

        public int Id { get; private set; }

        public bool IsActive { get; set; }

        public int InvitedUserId { get; set; }

        public int TeamId { get; set; }

        public virtual User InvitedUser { get; set; }

        public virtual Team Team { get; set; }
    }
}