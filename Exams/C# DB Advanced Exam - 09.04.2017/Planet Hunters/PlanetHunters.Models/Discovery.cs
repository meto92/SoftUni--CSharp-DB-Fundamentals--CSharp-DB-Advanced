using System;
using System.Collections.Generic;

namespace PlanetHunters.Models
{
    public class Discovery
    {
        public Discovery()
        {
            this.Stars = new HashSet<Star>();

            this.Planets = new HashSet<Planet>();

            this.Astronomers = new HashSet<AstronomerDiscovery>();

            this.Observers = new HashSet<ObserverDiscovery>();
        }

        public int Id { get; private set; }

        public DateTime DateMade { get; set; }

        public int TelescopeId { get; set; }

        public virtual Telescope Telescope { get; set; }

        public virtual ICollection<Star> Stars { get; set; }

        public virtual ICollection<Planet> Planets { get; set; }

        public virtual ICollection<AstronomerDiscovery> Astronomers { get; set; }

        public virtual ICollection<ObserverDiscovery> Observers { get; set; }
    }
}