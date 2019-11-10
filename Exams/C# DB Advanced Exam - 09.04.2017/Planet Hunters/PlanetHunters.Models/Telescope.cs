using System.Collections.Generic;

namespace PlanetHunters.Models
{
    public class Telescope
    {
        public Telescope()
        {
            this.Discoveries = new HashSet<Discovery>();
        }

        public int Id { get; private set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public double? MirrorDiameter { get; set; }

        public virtual ICollection<Discovery> Discoveries { get; set; }
    }
}