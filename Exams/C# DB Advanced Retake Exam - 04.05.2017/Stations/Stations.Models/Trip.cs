using System;
using System.Collections.Generic;
using Stations.Models.Enums;

namespace Stations.Models
{
    public class Trip
    {
        public Trip()
        {
            this.Tickets = new HashSet<Ticket>();
        }

        public int Id { get; private set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public TripStatus Status { get; set; }

        public TimeSpan? TimeDifference { get; set; }

        public int OriginStationId { get; set; }

        public int DestinationStationId { get; set; }

        public int TrainId { get; set; }

        public virtual Station OriginStation { get; set; }

        public virtual Station DestinationStation { get; set; }

        public virtual Train Train { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}