namespace PlanetHunters.Models
{
    public class AstronomerDiscovery
    {
        public int AstronomerId { get; set; }

        public int DiscoveryId { get; set; }

        public virtual Astronomer Astronomer { get; set; }

        public virtual Discovery Discovery { get; set; }
    }
}