using System.Collections.Generic;

namespace Stations.Models
{
    public class Station
    {
        public Station()
        {
            this.Departures = new HashSet<Trip>();

            this.ArrivingTrips = new HashSet<Trip>();
        }

        public int Id { get; private set; }

        public string Name { get; set; }

        public string Town { get; set; }

        public virtual ICollection<Trip> Departures { get; set; }

        public virtual ICollection<Trip> ArrivingTrips { get; set; }
    }
}