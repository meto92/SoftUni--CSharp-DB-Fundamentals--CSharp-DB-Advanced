using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusTicketsSystem.Models
{
    public class BusStation
    {
        public BusStation()
        {
            this.Arrivals = new HashSet<Trip>();
            this.Departures = new HashSet<Trip>();
        }

        public int Id { get; private set; }

        public string Name { get; set; }

        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        [InverseProperty("DestinationBusStation")]
        public virtual ICollection<Trip> Arrivals { get; set; }

        [InverseProperty("OriginBusStation")]
        public virtual ICollection<Trip> Departures { get; set; }
    }
}