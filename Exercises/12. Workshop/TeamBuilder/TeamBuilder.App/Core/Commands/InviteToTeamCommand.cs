using System;

using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Models;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.App.Core.Commands
{
    public class InviteToTeamCommand : ICommand
    {
        private readonly ITeamService teamService;
        private readonly IUserService userService;
        private readonly IInvitationService invitationService;

        public InviteToTeamCommand(
            ITeamService teamService, 
            IUserService userService,
            IInvitationService invitationService)
        {
            this.teamService = teamService;
            this.userService = userService;
            this.invitationService = invitationService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);

            string teamName = arguments[0];
            string username = arguments[1];

            bool teamExists = this.teamService.Exists(teamName);
            bool userExists = this.userService.Exists(username);

            if (!teamExists || !userExists)
            {
                throw new ArgumentException(Constants.ErrorMessages.TeamOrUserNotExist);
            }

            int currentUserId = AuthenticationManager.GetCurrentUser().Id;

            bool isCurrentUserCreatorOfTeam = this.teamService.IsUserCreatorOfTeam(teamName, currentUserId);
            bool isCurrentUserMemberOfTeam = this.teamService.IsUserMemberOfTeam(teamName, currentUserId);

            if (!isCurrentUserCreatorOfTeam && !isCurrentUserMemberOfTeam)
            {
                throw new InvalidCastException(Constants.ErrorMessages.NotAllowed);
            }

            int teamId = this.teamService.ByName<Team>(teamName).Id;
            int invitedUserId = this.userService.ByUsername<User>(username).Id;

            bool isInvitationSent = this.invitationService.Exists(teamId, invitedUserId);

            if (isInvitationSent)
            {
                throw new InvalidCastException(Constants.ErrorMessages.InviteIsAlreadySent);
            }

            this.invitationService.CreateInvitation(teamId, invitedUserId);

            string result = string.Format(
                Constants.SuccessMessages.InvitationSent, 
                teamName, username);

            return result;
        }
    }
}