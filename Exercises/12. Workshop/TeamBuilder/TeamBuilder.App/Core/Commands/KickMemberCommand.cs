using System;

using TeamBuilder.App.Core.DTOs;
using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.App.Core.Commands
{
    public class KickMemberCommand : ICommand
    {
        private readonly ITeamService teamService;
        private readonly IUserService userService;

        public KickMemberCommand(ITeamService teamService, IUserService userService)
        {
            this.teamService = teamService;
            this.userService = userService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);

            string teamName = arguments[0];
            string username = arguments[1];

            bool teamExists = this.teamService.Exists(teamName);

            if (!teamExists)
            {
                throw new ArgumentException(string.Format(
                    Constants.ErrorMessages.TeamNotFound,
                    teamName));
            }

            bool userExists = this.userService.Exists(username);

            if (!userExists)
            {
                throw new ArgumentException(string.Format(
                    Constants.ErrorMessages.UserNotFound,
                    username));
            }

            int userId = this.userService.ByUsername<UserDto>(username).Id;

            bool isUserMemberOfTeam = this.teamService.IsUserMemberOfTeam(teamName, userId);

            if (!isUserMemberOfTeam)
            {
                throw new ArgumentException(string.Format(
                    Constants.ErrorMessages.NotPartOfTeam,
                    username, teamName));
            }

            int currentUserId = AuthenticationManager.GetCurrentUser().Id;

            bool isCurrentUserCreatorOfTeam = this.teamService.IsUserCreatorOfTeam(teamName, currentUserId);

            if (!isCurrentUserCreatorOfTeam)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            int userToBeKickedId = this.userService.ByUsername<UserDto>(username).Id;

            bool isUserToBeKickedCreatorOfTheTea = this.teamService.IsUserCreatorOfTeam(teamName, userToBeKickedId);

            if (isUserToBeKickedCreatorOfTheTea)
            {
                throw new InvalidOperationException(string.Format( 
                    Constants.ErrorMessages.CommandNotAllowed,
                    Constants.DisbandTeam));
            }

            this.teamService.KickMember(teamName, userToBeKickedId);

            string result = string.Format(
                Constants.SuccessMessages.UserKicked, 
                username, teamName);

            return result;
        }
    }
}