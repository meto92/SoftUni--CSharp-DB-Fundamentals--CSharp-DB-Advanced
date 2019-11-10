using System;

namespace BusTicketsSystem.Models
{
    public class ArrivedTrip
    {
        public int Id { get; private set; }

        public DateTime ArrivalTime { get; set; }

        public int PassengersCount { get; set; }

        public int? OriginBusStationId { get; set; }

        public int? DestinationBusStationId { get; set; }

        public virtual BusStation OriginBusStation { get; set; }

        public virtual BusStation DestinationBusStation { get; set; }
    }
}