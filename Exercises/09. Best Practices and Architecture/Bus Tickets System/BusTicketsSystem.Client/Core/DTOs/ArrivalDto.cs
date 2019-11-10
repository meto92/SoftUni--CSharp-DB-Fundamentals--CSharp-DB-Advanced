using BusTicketsSystem.Models.Enums;

namespace BusTicketsSystem.Client.Core.DTOs
{
    public class ArrivalDto
    {
        public string OriginBusStationTown { get; set; }

        public int ArrivalHour { get; set; }

        public int ArrivalMinutes { get; set; }

        public Status Status { get; set; }
    }
}