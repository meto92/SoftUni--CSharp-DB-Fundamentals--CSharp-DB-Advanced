using BusTicketsSystem.Models;

namespace BusTicketsSystem.Services.Interfaces
{
    public interface ITicketService
    {
        Ticket CreateTicket(int customerId, int tripId, decimal price, string seat);
    }
}