using System;
using System.Collections.Generic;

using BusTicketsSystem.Models.Enums;

namespace BusTicketsSystem.Models
{
    public class Customer
    {
        public Customer()
        {
            this.Tickets = new HashSet<Ticket>();
            this.Reviews = new HashSet<Review>();
        }

        public int Id { get; private set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => this.FirstName + " " + this.LastName;

        public DateTime? DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public int? HomeTownId { get; set; }

        public virtual Town HomeTown { get; set; }

        public virtual BankAccount BankAccount { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}