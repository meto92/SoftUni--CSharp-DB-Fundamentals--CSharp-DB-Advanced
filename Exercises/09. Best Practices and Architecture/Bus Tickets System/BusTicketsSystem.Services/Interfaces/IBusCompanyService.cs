namespace BusTicketsSystem.Services.Interfaces
{
    public interface IBusCompanyService
    {
        TModel ById<TModel>(int id);

        TModel ByName<TModel>(string name);

        bool Exists(int id);

        bool Exists(string name);
    }
}