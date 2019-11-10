namespace PhotoShare.Client.Core.Commands
{
    using System;
    using PhotoShare.Client.Utilities;
    using Services.Contracts;

    public class AddTownCommand : Command
    {
        private const string TownNameAlreadyAddedMessage = "Town {0} was already added!";
        private const string TownSuccessfullyAddedMessage = "Town {0} was added successfully!";

        private readonly ITownService townService;

        public AddTownCommand(ITownService townService)
        {
            this.townService = townService;
        }

        // AddTown <townName> <countryName>
        public override string Execute(string[] data)
        {
            string townName = data[0];
            string country = data[1];

            UserUtilities.ValidateLoggedInUserAction(Session.User?.Username);

            bool townExists = this.townService.Exists(townName);

            if (townExists)
            {
                throw new ArgumentException(string.Format(
                    TownNameAlreadyAddedMessage,
                    townName));
            }

            var town = this.townService.Add(townName, country);

            string result = string.Format(TownSuccessfullyAddedMessage, townName);

            return result;
        }
    }
}