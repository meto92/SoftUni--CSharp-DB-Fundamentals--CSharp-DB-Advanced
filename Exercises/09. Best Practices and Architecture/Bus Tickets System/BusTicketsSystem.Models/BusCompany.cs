using System.Collections.Generic;

namespace BusTicketsSystem.Models
{
    public class BusCompany
    {
        public BusCompany()
        {
            this.Trips = new HashSet<Trip>();
            this.Reviews = new HashSet<Review>();
        }

        public int Id { get; private set; }

        public string Name { get; set; }

        public string Nationality { get; set; }

        public float Rating { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}