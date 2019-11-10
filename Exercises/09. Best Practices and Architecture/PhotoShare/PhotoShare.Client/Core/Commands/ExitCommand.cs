namespace PhotoShare.Client.Core.Commands
{
    using System;

    public class ExitCommand : Command
    {
        private const string ExitMessage = "Bye-bye!";

        public override int RequiredArgumentsCount => 0;

        public override string Execute(string[] data)
        {
            Console.WriteLine(ExitMessage);

            Environment.Exit(0);

            return null;
        }
    }
}