using System;

using BusTicketsSystem.Models.Enums;

namespace BusTicketsSystem.Client.Core.DTOs
{
    public class TripChangeStatusDto
    {
        public string OriginBusStationTown { get; set; }

        public string DestinationBusStationTown { get; set; }

        public Status Status { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public int PassengersCount { get; set; }
    }
}