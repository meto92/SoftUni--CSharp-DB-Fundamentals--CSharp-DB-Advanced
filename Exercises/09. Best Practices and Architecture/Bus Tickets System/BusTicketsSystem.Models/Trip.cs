using System;
using System.Collections.Generic;

using BusTicketsSystem.Models.Enums;

namespace BusTicketsSystem.Models
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

        public Status Status { get; set; }

        public int OriginBusStationId { get; set; }

        public int DestinationBusStationId { get; set; }

        public int BusCompanyId { get; set; }

        public virtual BusStation OriginBusStation { get; set; }

        public virtual BusStation DestinationBusStation { get; set; }

        public virtual BusCompany BusCompany { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}