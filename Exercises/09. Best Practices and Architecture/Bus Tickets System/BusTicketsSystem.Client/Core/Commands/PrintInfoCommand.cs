using System;
using System.Linq;
using System.Text;

using BusTicketsSystem.Client.Core.DTOs;
using BusTicketsSystem.Client.Core.Interfaces;
using BusTicketsSystem.Services.Interfaces;

namespace BusTicketsSystem.Client.Core.Commands
{
    public class PrintInfoCommand : ICommand
    {
        private const string BusStationNotFoundMEssage = "Bus station with Id {0} not found!";

        private IBusStationService busStationService;

        public PrintInfoCommand(IBusStationService busStationService)
        {
            this.busStationService = busStationService;
        }

        private string GetInfo(BusStationDto busStation)
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine($"{busStation.Name}, {busStation.Town}");

            result.AppendLine("Arrivals:");

            if (busStation.Arrivals.Count == 0)
            {
                result.AppendLine("None");
            }

            busStation.Arrivals
                .Select(t => string.Format("From: {0} | Arrive at: {1:d2}:{2:d2} | Status: {3}",
                    t.OriginBusStationTown,
                    t.ArrivalHour,
                    t.ArrivalMinutes,
                    t.Status))
                .ToList()
                .ForEach(a => result.AppendLine(a));

            result.AppendLine("Departures:");

            if (busStation.Departures.Count == 0)
            {
                result.AppendLine("None");
            }

            busStation.Departures
                .Select(t => string.Format("To {0} | Depart at: {1:d2}:{2:d2} | Status: {3}",
                    t.DestinationBusStationTown,
                    t.DepartureHour,
                    t.DepartureMinutes,
                    t.Status))
                .ToList()
                .ForEach(t => result.AppendLine(t));

            return result.ToString().TrimEnd();
        }

        public string Execute(string[] args)
        {
            int busStationId = int.Parse(args[0]);

            bool busStationExists = this.busStationService.Exists(busStationId);

            if (!busStationExists)
            {
                throw new ArgumentException(string.Format(
                    BusStationNotFoundMEssage,
                    busStationId));
            }

            BusStationDto busStation = this.busStationService.ById<BusStationDto>(busStationId);

            string result = GetInfo(busStation);
            
            return result;
        }
    }
}