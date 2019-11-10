using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper.QueryableExtensions;

using BusTicketsSystem.Data;
using BusTicketsSystem.Models;
using BusTicketsSystem.Services.Interfaces;

namespace BusTicketsSystem.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly BusTicketsSystemContext db; 
        private readonly ITicketService ticketService;

        public CustomerService(BusTicketsSystemContext db, ITicketService ticketService)
        {
            this.db = db;
            this.ticketService = ticketService;
        }

        private IEnumerable<TModel> By<TModel>(Func<Customer, bool> predicate) =>
            this.db.Customers
                .Where(predicate)
                .AsQueryable()
                .ProjectTo<TModel>();

        public TModel ById<TModel>(int id) 
            => this.By<TModel>(c => c.Id == id).SingleOrDefault();

        public bool Exists(int id) => ById<Customer>(id) != null;

        public void BuyTicket(int customerId, int tripId, decimal price, string seat)
        {
            Customer customer = this.ById<Customer>(customerId);

            Ticket ticket = this.ticketService.CreateTicket(customerId, tripId, price, seat);

            customer.BankAccount.Balance -= price;

            customer.Tickets.Add(ticket);

            this.db.SaveChanges();
        }
    }
}