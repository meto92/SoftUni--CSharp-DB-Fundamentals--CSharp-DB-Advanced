using System.Collections.Generic;

using Stations.Models.Enums;

namespace Stations.Models
{
    public class Train
    {
        public Train()
        {
            this.Trips = new HashSet<Trip>();

            this.Seats = new HashSet<TrainSeats>();
        }

        public int Id { get; private set; }

        public string TrainNumber { get; set; }

        public TrainType? Type { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }

        public virtual ICollection<TrainSeats> Seats { get; set; }
    }
}