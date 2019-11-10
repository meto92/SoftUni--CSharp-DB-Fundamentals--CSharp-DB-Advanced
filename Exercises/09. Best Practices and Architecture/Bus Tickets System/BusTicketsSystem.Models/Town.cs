using System.Collections.Generic;

namespace BusTicketsSystem.Models
{
    public class Town
    {
        public Town()
        {
            this.Customers = new HashSet<Customer>();
            this.BusStations = new HashSet<BusStation>();
        }

        public int Id { get; private set; }
        
        public string Name { get; set; }

        public string Country { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }

        public virtual ICollection<BusStation> BusStations { get; set; }
    }
}