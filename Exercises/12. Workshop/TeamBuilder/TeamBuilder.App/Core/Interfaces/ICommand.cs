namespace TeamBuilder.App.Core.Interfaces
{
    public interface ICommand
    {
        string Execute(string[] arguments);
    }
}