using System.Collections.Generic;

namespace Stations.Models
{
    public class SeatingClass
    {
        public SeatingClass()
        {
            this.Seats = new HashSet<TrainSeats>();
        }

        public int Id { get; private set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public virtual ICollection<TrainSeats> Seats { get; set; }
    }
}