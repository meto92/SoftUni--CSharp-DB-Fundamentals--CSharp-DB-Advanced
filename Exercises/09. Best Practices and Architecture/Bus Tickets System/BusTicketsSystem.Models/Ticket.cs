namespace BusTicketsSystem.Models
{
    public class Ticket
    {
        public int Id { get; private set; }

        public decimal Price { get; set; }

        public string Seat { get; set; }

        public int CustomerId { get; set; }

        public int TripId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Trip Trip { get; set; }
    }
}