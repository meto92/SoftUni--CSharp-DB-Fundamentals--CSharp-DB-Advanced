using System.Collections.Generic;

namespace BusTicketsSystem.Client.Core.DTOs
{
    public class BusStationDto
    {
        public string Name { get; set; }

        public string Town { get; set; }

        public ICollection<ArrivalDto> Arrivals { get; set; }

        public ICollection<DepartureDto> Departures { get; set; }
    }
}