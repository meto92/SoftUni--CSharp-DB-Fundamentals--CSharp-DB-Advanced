using System;

using BusTicketsSystem.Client.Core.DTOs;
using BusTicketsSystem.Client.Core.Interfaces;
using BusTicketsSystem.Models.Enums;
using BusTicketsSystem.Services.Interfaces;

namespace BusTicketsSystem.Client.Core.Commands
{
    public class ChangeTripStatusCommand : ICommand
    {
        private const string TripNotFoundMessage = "Trip with Id {0} not found!";
        private const string InvalidStatusMessage = "Invalid status!";
        private const string TripAlreadyWithSameStatusMessage = "Trip is already {0}";
        private const string CannotChangeStatusOfArrivedTripMessage = "Cannot change status of arrived trip!";

        private readonly ITripService tripService;

        public ChangeTripStatusCommand(ITripService tripService)
        {
            this.tripService = tripService;
        }

        public string Execute(string[] args)
        {
            int tripId = int.Parse(args[0]);
            string newStatusStr = args[1];

            bool tripExists = this.tripService.Exists(tripId);

            if (!tripExists)
            {
                throw new ArgumentException(string.Format(
                    TripNotFoundMessage,
                    tripId));
            }

            if (!Enum.TryParse(newStatusStr, out Status newStatus))
            {
                throw new ArgumentException(InvalidStatusMessage);
            }

            TripChangeStatusDto trip = this.tripService.ById<TripChangeStatusDto>(tripId);

            Status oldStatus = trip.Status;

            if (oldStatus == newStatus)
            {
                throw new InvalidOperationException(string.Format(
                    TripAlreadyWithSameStatusMessage,
                    newStatusStr));
            }

            if (oldStatus == Status.Arrived)
            {
                throw new InvalidOperationException(CannotChangeStatusOfArrivedTripMessage);
            }

            this.tripService.ChangeStatus(tripId, newStatusStr);

            string result = string.Format(
                "Trip from {0} to {1} on {2}{3}Status changed from {4} to {5}",
                trip.OriginBusStationTown,
                trip.DestinationBusStationTown,
                trip.DepartureTime,
                Environment.NewLine,
                oldStatus,
                newStatus);

            if (newStatus == Status.Arrived)
            {
                result += string.Format(
                    "{0}On {1} - {2} passengers arrived at {3} from {4}",
                    Environment.NewLine,
                    trip.ArrivalTime,
                    trip.PassengersCount,
                    trip.DestinationBusStationTown,
                    trip.OriginBusStationTown);
            }

            return result;
        }
    }
}