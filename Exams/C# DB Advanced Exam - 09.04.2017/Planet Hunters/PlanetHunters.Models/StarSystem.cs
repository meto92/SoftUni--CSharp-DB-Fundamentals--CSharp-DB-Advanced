using System.Collections.Generic;

namespace PlanetHunters.Models
{
    public class StarSystem
    {
        public StarSystem()
        {
            this.Stars = new HashSet<Star>();

            this.Planets = new HashSet<Planet>();
        }

        public int Id { get; private set; }

        public string Name { get; set; }

        public virtual ICollection<Star> Stars { get; set; }

        public virtual ICollection<Planet> Planets { get; set; }
    }
}