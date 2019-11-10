using System;

using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.App.Core.Commands
{
    public class DisbandCommand : ICommand
    {
        private readonly ITeamService teamService;

        public DisbandCommand(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(1, arguments);

            string teamName = arguments[0];

            bool teamExists = this.teamService.Exists(teamName);

            if (!teamExists)
            {
                throw new ArgumentException(string.Format(
                    Constants.ErrorMessages.TeamNotFound,
                    teamName));
            }

            int currentUserId = AuthenticationManager.GetCurrentUser().Id;

            bool isCurrentUserCreatorOfTheTeam = this.teamService.IsUserCreatorOfTeam(teamName, currentUserId);

            if (!isCurrentUserCreatorOfTheTeam)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            this.teamService.Disband(teamName);

            string result = string.Format(
                Constants.SuccessMessages.TeamDisbanded, 
                teamName);

            return result;
        }
    }
}