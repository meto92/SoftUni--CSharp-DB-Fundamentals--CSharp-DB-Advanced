using System;

namespace BusTicketsSystem.Models
{
    public class Review
    {
        public int Id { get; private set; }

        public string Content { get; set; }

        public float Grade { get; set; }

        public DateTime PublishedOn { get; set; }

        public int BusCompanyId { get; set; }

        public int CustomerId { get; set; }

        public virtual BusCompany BusCompany { get; set; }

        public virtual Customer Customer { get; set; }
    }
}