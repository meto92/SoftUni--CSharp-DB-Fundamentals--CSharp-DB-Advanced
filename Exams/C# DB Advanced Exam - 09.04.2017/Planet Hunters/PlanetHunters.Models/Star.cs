namespace PlanetHunters.Models
{
    public class Star
    {
        public int Id { get; private set; }

        public string Name { get; set; }

        public int Temperature { get; set; }

        public int HostStarSystemId { get; set; }

        public int? DiscoveryId { get; set; }

        public virtual StarSystem HostStarSystem { get; set; }

        public virtual Discovery Discovery { get; set; }
    }
}