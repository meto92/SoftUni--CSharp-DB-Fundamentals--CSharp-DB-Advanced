using System;
using System.Collections.Generic;
using System.Linq;
using FastFood.Models.Enums;

namespace FastFood.Models
{
    public class Order
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }

        public string Customer { get; set; }

        public OrderType Type { get; set; }

        public DateTime DateTime { get; set; }

        public decimal TotalPrice => this.OrderItems.Sum(oi => oi.Quantity * oi.Item.Price);

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}