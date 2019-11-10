using System;
using System.Linq;

using BusTicketsSystem.Client.Core.DTOs;
using BusTicketsSystem.Client.Core.Interfaces;
using BusTicketsSystem.Services.Interfaces;

namespace BusTicketsSystem.Client.Core.Commands
{
    public class BuyTicketCommand : ICommand
    {
        private const string CustomerNotFoundMEssage = "Customer with Id {0} not found!";
        private const string TripNotFoundMEssage = "Trip with Id {0} not found!";
        private const string InvalidPriceMessage = "Invalid price!";
        private const string InsufficientFundsMessage = "Insufficient amount of money for customer {0} with bank account number {1}";
        private const string SeatAlreadyTakenMessage = "Seat {0} for trip with Id {1} is already taken!";
        private const string SuccessMessage = "Customer {0} bought ticket for trip {1} for ${2:f2} on seat {3}";

        private readonly ICustomerService customerService;
        private readonly ITripService tripService;

        public BuyTicketCommand(ICustomerService customerService, ITripService tripService)
        {
            this.customerService = customerService;
            this.tripService = tripService;
        }

        public string Execute(string[] args)
        {
            int customerId = int.Parse(args[0]);
            int tripId = int.Parse(args[1]);
            decimal price = decimal.Parse(args[2]);
            string seat = args[3];

            bool customerExists = this.customerService.Exists(customerId);

            if (!customerExists)
            {
                throw new ArgumentException(string.Format(
                    CustomerNotFoundMEssage,
                    customerId));
            }

            bool tripExists = this.tripService.Exists(tripId);

            if (!tripExists)
            {
                throw new ArgumentException(string.Format(
                    TripNotFoundMEssage,
                    tripId));
            }

            if (price <= 0)
            {
                throw new ArgumentException(InvalidPriceMessage);
            }

            CustomerDto customer = this.customerService.ById<CustomerDto>(customerId);

            if (customer.Balance < price)
            {
                throw new InvalidOperationException(string.Format(
                    InsufficientFundsMessage,
                    customer.FullName, customer.BankAccountNumber));
            }

            TripDto trip = this.tripService.ById<TripDto>(tripId);

            if (trip.Tickets.Any(t => t.Seat == seat))
            {
                throw new ArgumentException(string.Format(
                    SeatAlreadyTakenMessage,
                    seat, tripId));
            }

            this.customerService.BuyTicket(customerId, tripId, price, seat);

            string result = string.Format(
                SuccessMessage,
                customer.FullName, tripId, price, seat);

            return result;
        }
    }
}