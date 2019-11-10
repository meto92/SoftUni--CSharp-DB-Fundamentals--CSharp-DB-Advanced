using System.Collections.Generic;

namespace FastFood.Models
{
	public class Employee
	{
        public Employee()
        {
            this.Orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        public int Age { get; set; }

        public string Name { get; set; }

        public int PositionId { get; set; }

        public Position Position { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}