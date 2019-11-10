using BusTicketsSystem.Models;
using BusTicketsSystem.Services.Interfaces;

namespace BusTicketsSystem.Services
{
    public class TicketService : ITicketService
    {
        public Ticket CreateTicket(int customerId, int tripId, decimal price, string seat)
        {
            Ticket ticket = new Ticket
            {
                Price = price,
                Seat = seat,
                CustomerId = customerId,
                TripId = tripId
            };

            return ticket;
        }
    }
}