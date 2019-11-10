using System;
using System.Linq;
using BusTicketsSystem.Client.Core.DTOs;
using BusTicketsSystem.Client.Core.Interfaces;
using BusTicketsSystem.Services.Interfaces;

namespace BusTicketsSystem.Client.Core.Commands
{
    public class PublishReviewCommand : ICommand
    {
        private const string CustomerNotFoundMEssage = "Customer with Id {0} not found!";
        private const string BusCompanyNotFoundMessage = "Bus company {0} not found!";
        private const string InvalidGradeException = "Grade must be between 1 and 10!";
        private const string SuccessMessage = "{0}'s review was succesfully published";

        private readonly ICustomerService customerService;
        private readonly IBusCompanyService busCompanyService;
        private readonly IReviewService reviewService;

        public PublishReviewCommand(
            ICustomerService customerService, 
            IBusCompanyService busCompanyService,
            IReviewService reviewService)
        {
            this.customerService = customerService;
            this.busCompanyService = busCompanyService;
            this.reviewService = reviewService;
        }

        public string Execute(string[] args)
        {
            int customerId = int.Parse(args[0]);
            float grade = float.Parse(args[1]);
            string busCompanyName = args[2];
            string content = string.Join(" ", args.Skip(3));

            bool customerExists = this.customerService.Exists(customerId);

            if (!customerExists)
            {
                throw new ArgumentException(string.Format(
                    CustomerNotFoundMEssage,
                    customerId));
            }

            bool busCompanyExists = this.busCompanyService.Exists(busCompanyName);

            if (!busCompanyExists)
            {
                throw new ArgumentException(string.Format(
                    BusCompanyNotFoundMessage,
                    busCompanyName));
            }

            if (grade < 1 || grade > 10)
            {
                throw new ArgumentException(InvalidGradeException);
            }

            int busCompanyId = this.busCompanyService.ByName<BusCompanyDto>(busCompanyName).Id;

            this.reviewService.CreateReview(customerId, grade, busCompanyId, content);

            string customerFullName = this.customerService.ById<CustomerDto>(customerId).FullName;

            string result = string.Format(SuccessMessage, customerFullName);

            return result;
        }
    }
}