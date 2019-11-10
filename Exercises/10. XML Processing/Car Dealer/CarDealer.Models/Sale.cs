namespace CarDealer.Models
{
    public class Sale
    {
        public int Id { get; private set; }

        public int Discount { get; set; }

        public int CarId { get; set; }

        public int CustomerId { get; set; }

        public virtual Car Car { get; set; }

        public virtual Customer Customer { get; set; }
    }
}