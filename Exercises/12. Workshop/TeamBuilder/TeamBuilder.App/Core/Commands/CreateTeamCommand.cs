using System;

using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.App.Core.Commands
{
    public class CreateTeamCommand : ICommand
    {
        private readonly ITeamService teamService;

        public CreateTeamCommand(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);

            string teamName = arguments[0];
            string acronym = arguments[1];
            string description = arguments.Length > 2 ? arguments[2] : null;

            bool teamExists = this.teamService.Exists(teamName);

            if (teamExists)
            {
                throw new ArgumentException(string.Format(
                    Constants.ErrorMessages.TeamExists,
                    teamName));
            }

            if (acronym.Length != 3)
            {
                throw new ArgumentException(string.Format(
                    Constants.ErrorMessages.InvalidAcronym,
                    acronym));
            }

            int creatorId = AuthenticationManager.GetCurrentUser().Id;

            this.teamService.CreateTeam(teamName, creatorId, acronym, description);

            string result = string.Format(
                Constants.SuccessMessages.TeamCreated, 
                teamName);

            return result;
        }
    }
}