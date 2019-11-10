namespace Stations.Models
{
    public class Ticket
    {
        public int Id { get; private set; }

        public decimal Price { get; set; }

        public string SeatingPlace { get; set; }

        public int TripId { get; set; }

        public int? PersonalCardId { get; set; }

        public virtual Trip Trip { get; set; }

        public virtual CustomerCard PersonalCard { get; set; }
    }
}