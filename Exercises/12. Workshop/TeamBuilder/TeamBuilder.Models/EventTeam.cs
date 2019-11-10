namespace TeamBuilder.Models
{
    public class EventTeam
    {
        public int EventId { get; set; }

        public int TeamId { get; set; }

        public virtual Event Event { get; set; }

        public virtual Team Team { get; set; }
    }
}