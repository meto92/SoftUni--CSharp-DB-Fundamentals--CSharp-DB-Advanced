using System.Collections.Generic;
using Stations.Models.Enums;

namespace Stations.Models
{
    public class CustomerCard
    {
        public CustomerCard()
        {
            this.Tickets = new HashSet<Ticket>();
        }

        public int Id { get; private set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public CardType Type { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}