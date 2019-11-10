using System.Collections.Generic;

namespace PlanetHunters.Models
{
    public class Astronomer
    {
        public Astronomer()
        {
            this.PioneeringDiscoveries = new HashSet<AstronomerDiscovery>();

            this.ObservationsOfDiscoveries = new HashSet<ObserverDiscovery>();
        }

        public int Id { get; private set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => this.FirstName + " " + this.LastName;

        public virtual ICollection<AstronomerDiscovery> PioneeringDiscoveries { get; set; }

        public virtual ICollection<ObserverDiscovery> ObservationsOfDiscoveries { get; set; }
    }
}