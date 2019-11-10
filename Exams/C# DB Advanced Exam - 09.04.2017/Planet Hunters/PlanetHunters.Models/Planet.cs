namespace PlanetHunters.Models
{
    public class Planet
    {
        public int Id { get; private set; }

        public string Name { get; set; }

        public double Mass { get; set; }

        public int HostStarSystemId { get; set; }

        public int? DiscoveryId { get; set; }

        public virtual StarSystem HostStarSystem { get; set; }

        public virtual Discovery Discovery { get; set; }
    }
}