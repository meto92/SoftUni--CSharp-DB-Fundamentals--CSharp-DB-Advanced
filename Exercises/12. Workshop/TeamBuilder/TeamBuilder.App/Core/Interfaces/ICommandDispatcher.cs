namespace TeamBuilder.App.Core.Interfaces
{
    public interface ICommandDispatcher
    {
        string Dispatch(string input);
    }
}