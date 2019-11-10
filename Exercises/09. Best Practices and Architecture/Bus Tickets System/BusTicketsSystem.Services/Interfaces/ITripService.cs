namespace BusTicketsSystem.Services.Interfaces
{
    public interface ITripService
    {
        TModel ById<TModel>(int id);

        bool Exists(int id);

        void ChangeStatus(int tripId, string newStatusStr);
    }
}