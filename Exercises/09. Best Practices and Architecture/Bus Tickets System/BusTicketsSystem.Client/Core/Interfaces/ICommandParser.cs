namespace BusTicketsSystem.Client.Core.Interfaces
{
    public interface ICommandParser
    {
        ICommand ParseCommand(string commandName);
    }
}