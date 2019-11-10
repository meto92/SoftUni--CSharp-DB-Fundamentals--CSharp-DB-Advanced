using System.Collections.Generic;
using System.Linq;

namespace CarDealer.Models
{
    public class Car
    {
        public Car()
        {
            this.CarParts = new HashSet<PartCar>();
        }

        public int Id { get; private set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        public decimal Price => this.CarParts.Sum(cp => cp.Part.Price);

        public virtual ICollection<PartCar> CarParts { get; set; }

        public virtual Sale Sale { get; set; }
    }
}