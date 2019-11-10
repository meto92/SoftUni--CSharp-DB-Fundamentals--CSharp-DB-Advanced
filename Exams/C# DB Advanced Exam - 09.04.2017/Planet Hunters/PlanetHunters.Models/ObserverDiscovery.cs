namespace PlanetHunters.Models
{
    public class ObserverDiscovery
    {
        public int ObserverId { get; set; }

        public int DiscoveryId { get; set; }

        public virtual Astronomer Observer { get; set; }

        public virtual Discovery Discovery { get; set; }
    }
}