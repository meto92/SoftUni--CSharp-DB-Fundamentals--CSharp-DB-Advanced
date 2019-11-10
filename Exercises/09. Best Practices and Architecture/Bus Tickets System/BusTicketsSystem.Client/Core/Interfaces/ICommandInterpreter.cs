namespace BusTicketsSystem.Client.Core.Interfaces
{
    public interface ICommandInterpreter
    {
        string InterpretCommand(string[] input);
    }
}