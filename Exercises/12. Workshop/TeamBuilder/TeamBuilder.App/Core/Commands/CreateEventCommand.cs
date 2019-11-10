using System;
using System.Globalization;
using System.Linq;

using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.App.Core.Commands
{
    public class CreateEventCommand : ICommand
    {
        private readonly IEventService eventService;

        public CreateEventCommand(IEventService eventService)
        {
            this.eventService = eventService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(6, arguments);

            string eventName = arguments[0];
            string description = arguments[1];
            string startDateStr = string.Join(" ", arguments.Skip(2).Take(2));
            string endDateStr = string.Join(" ", arguments.Skip(4).Take(2));

            bool isStartDateStrValid = DateTime.TryParseExact(startDateStr, Constants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startdate);
            bool isEndDateStrValid = DateTime.TryParseExact(endDateStr, Constants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate);

            if (!isStartDateStrValid || !isEndDateStrValid)
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidDateFormat);
            }

            if (startdate > endDate)
            {
                throw new ArgumentException(Constants.ErrorMessages.StartDateShouldBeBeforeEndDate);
            }

            int creatorId = AuthenticationManager.GetCurrentUser().Id;

            this.eventService.CreateEvent(eventName, creatorId, description, startdate, endDate);

            string result = string.Format(
                Constants.SuccessMessages.EventCreated, 
                eventName);

            return result;
        }
    }
}