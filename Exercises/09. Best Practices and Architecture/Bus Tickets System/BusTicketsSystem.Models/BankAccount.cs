namespace BusTicketsSystem.Models
{
    public class BankAccount
    {
        public int Id { get; private set; }

        public string AccountNumber { get; set; }

        public decimal Balance { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}