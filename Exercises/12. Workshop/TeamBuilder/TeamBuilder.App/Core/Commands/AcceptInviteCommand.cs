using System;

using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Models;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.App.Core.Commands
{
    public class AcceptInviteCommand : ICommand
    {
        private readonly ITeamService teamService;
        private readonly IInvitationService invitationService;

        public AcceptInviteCommand(
            ITeamService teamService, 
            IInvitationService invitationService)
        {
            this.teamService = teamService;
            this.invitationService = invitationService;
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
            int teamId = this.teamService.ByName<Team>(teamName).Id;

            bool invitationExists = this.invitationService.Exists(teamId, currentUserId);

            if (!invitationExists)
            {
                throw new ArgumentException(string.Format(
                    Constants.ErrorMessages.InviteNotFound,
                    teamName));
            }

            this.teamService.AddMember(teamId, currentUserId);

            string currentUserUsername = AuthenticationManager.GetCurrentUser().Username;

            int invitationId = this.invitationService.ByTeamIdAndInvitedUserId(teamId, currentUserId).Id;

            this.invitationService.Deactivate(invitationId);

            string result = string.Format(
                Constants.SuccessMessages.InvitationAccepted,
                currentUserUsername, teamName);

            return result;
        }
    }
}