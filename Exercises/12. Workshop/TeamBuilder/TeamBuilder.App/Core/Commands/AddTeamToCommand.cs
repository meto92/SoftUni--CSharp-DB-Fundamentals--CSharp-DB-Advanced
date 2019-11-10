using System;

using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Models;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.App.Core.Commands
{
    public class AddTeamToCommand : ICommand
    {
        private readonly ITeamService teamService;
        private readonly IEventService eventService;

        public AddTeamToCommand(ITeamService teamService, IEventService eventService)
        {
            this.teamService = teamService;
            this.eventService = eventService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);

            string eventName = arguments[0];
            string teamName = arguments[1];

            bool eventExists = this.eventService.Exists(eventName);

            if (!eventExists)
            {
                throw new ArgumentException(string.Format(
                    Constants.ErrorMessages.EventNotFound,
                    eventName));
            }

            bool teamExists = this.teamService.Exists(teamName);

            if (!teamExists)
            {
                throw new ArgumentException(string.Format(
                    Constants.ErrorMessages.TeamNotFound,
                    teamName));
            }

            int currentUserId = AuthenticationManager.GetCurrentUser().Id;

            bool isCurrentUserCreatorOfTeam = this.teamService.IsUserCreatorOfTeam(teamName, currentUserId);

            if (!isCurrentUserCreatorOfTeam)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            bool isTeamInEvent = this.eventService.IsTeamInEvent(eventName, teamName);

            if (isTeamInEvent)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.CannotAddSameTeamTwice);
            }

            int teamId = this.teamService.ByName<Team>(teamName).Id;

            this.eventService.AddTeamToEvent(eventName, teamId);

            string result = string.Format(
                Constants.SuccessMessages.TeamAddedToEventMessage, 
                teamName, eventName);

            return result;
        }
    }
}