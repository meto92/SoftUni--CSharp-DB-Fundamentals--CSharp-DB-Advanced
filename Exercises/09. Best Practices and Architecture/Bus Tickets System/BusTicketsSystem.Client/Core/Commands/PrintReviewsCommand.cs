using System;
using System.Linq;
using System.Text;

using BusTicketsSystem.Client.Core.DTOs;
using BusTicketsSystem.Client.Core.Interfaces;
using BusTicketsSystem.Services.Interfaces;

namespace BusTicketsSystem.Client.Core.Commands
{
    public class PrintReviewsCommand : ICommand
    {
        private const string BusCompanyNotFoundMessage = "Bus company with Id {0} not found!";

        private readonly IBusCompanyService busCompanyService;

        public PrintReviewsCommand(IBusCompanyService busCompanyService)
        {
            this.busCompanyService = busCompanyService;
        }

        private string GetReviewsInfo(BusCompanyReviewsDto busCompany)
        {
            StringBuilder result = new StringBuilder();

            busCompany.Reviews
                .Select(r => string.Format("{0} {1} {2}{5}{3}{5}{4}",
                    r.Id,
                    r.Grade,
                    r.PublishedOn,
                    r.CustomerFullName,
                    r.Content,
                    Environment.NewLine))
                .ToList()
                .ForEach(r => result.AppendLine(r));

            return result.ToString().TrimEnd();
        }

        public string Execute(string[] args)
        {
            int busCompanyId = int.Parse(args[0]);

            bool busCompanyExists = this.busCompanyService.Exists(busCompanyId);

            if (!busCompanyExists)
            {
                throw new ArgumentException(string.Format(
                    BusCompanyNotFoundMessage,
                    busCompanyId));
            }

            BusCompanyReviewsDto busCompany = this.busCompanyService.ById<BusCompanyReviewsDto>(busCompanyId);

            string result = GetReviewsInfo(busCompany);

            return result;
        }
    }
}