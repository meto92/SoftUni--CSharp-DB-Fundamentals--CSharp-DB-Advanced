namespace BusTicketsSystem.Services.Interfaces
{
    public interface IBusStationService
    {
        TModel ById<TModel>(int id);

        bool Exists(int id);
    }
}