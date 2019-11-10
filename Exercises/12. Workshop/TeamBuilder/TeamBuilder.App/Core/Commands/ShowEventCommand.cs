using System;
using System.Linq;
using System.Text;

using TeamBuilder.App.Core.DTOs;
using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.App.Core.Commands
{
    public class ShowEventCommand : ICommand
    {
        private readonly IEventService eventService;

        public ShowEventCommand(IEventService eventService)
        {
            this.eventService = eventService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(1, arguments);

            string eventName = arguments[0];

            bool eventExists = this.eventService.Exists(eventName);

            if (!eventExists)
            {
                throw new ArgumentException(string.Format(
                    Constants.ErrorMessages.EventNotFound,
                    eventName));
            }

            EventWithTeamsDto eventDto = this.eventService.ByName<EventWithTeamsDto>(eventName);

            StringBuilder result = new StringBuilder();

            result.AppendFormat("{0} {1} {2}{3}",
                eventDto.Name,
                eventDto.StartDate.ToString(Constants.DateTimeFormat),
                eventDto.EndDate.ToString(Constants.DateTimeFormat),
                Environment.NewLine);

            result.AppendLine(eventDto.Description);
            result.AppendLine("Teams:");

            result.Append(string.Join(
                Environment.NewLine, 
                eventDto.ParticipatingTeamNames.Select(teamName => $"-{teamName}")));

            return result.ToString().TrimEnd();
        }
    }
}