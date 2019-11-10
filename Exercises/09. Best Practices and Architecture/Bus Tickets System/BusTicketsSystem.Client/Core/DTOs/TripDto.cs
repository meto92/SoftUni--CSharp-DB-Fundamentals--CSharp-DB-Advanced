using System.Collections.Generic;

namespace BusTicketsSystem.Client.Core.DTOs
{
    public class TripDto
    {
        public int Id { get; set; }

        public ICollection<TicketDto> Tickets { get; set; }
    }
}