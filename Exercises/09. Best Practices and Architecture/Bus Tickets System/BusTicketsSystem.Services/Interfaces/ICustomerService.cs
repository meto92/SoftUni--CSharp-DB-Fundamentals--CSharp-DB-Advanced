namespace BusTicketsSystem.Services.Interfaces
{
    public interface ICustomerService
    {
        TModel ById<TModel>(int id);

        bool Exists(int id);

        void BuyTicket(int customerId, int tripId, decimal price, string seat);
    }
}