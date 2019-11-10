using TeamBuilder.Models.Enums;

namespace TeamBuilder.Services.Interfaces
{
    public interface IUserService
    {
        bool Exists(string username);

        TModel ByUsername<TModel>(string username);

        TModel ById<TModel>(int id);

        void Register(string username, string password, string firstName, string lastName, int age, Gender gender);

        TModel GetUserByCredentials<TModel>(string username, string password);

        void Delete(int id);
    }
}