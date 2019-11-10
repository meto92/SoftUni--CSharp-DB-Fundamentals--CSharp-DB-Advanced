using PhotoShare.Client.Core.Contracts;

namespace PhotoShare.Client.Core.Commands
{
    public abstract class Command : ICommand
    {
        private const int defaultRequiredArgumentsCount = 2;

        public virtual int RequiredArgumentsCount => defaultRequiredArgumentsCount;

        public abstract string Execute(string[] args);
    }
}