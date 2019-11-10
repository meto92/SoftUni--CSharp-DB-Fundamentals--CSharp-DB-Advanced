using BusTicketsSystem.Models.Enums;

namespace BusTicketsSystem.Client.Core.DTOs
{
    public class DepartureDto
    {
        public string DestinationBusStationTown { get; set; }

        public int DepartureHour { get; set; }

        public int DepartureMinutes { get; set; }

        public Status Status { get; set; }
    }
}