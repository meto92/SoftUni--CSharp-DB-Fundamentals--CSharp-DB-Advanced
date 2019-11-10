namespace PhotoShare.Client.Core.Contracts
{
    public interface ICommand
    {
        int RequiredArgumentsCount { get; }

        string Execute(string[] args);
    }
}