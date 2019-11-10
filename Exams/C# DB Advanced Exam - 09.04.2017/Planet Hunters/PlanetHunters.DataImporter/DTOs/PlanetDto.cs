using System.ComponentModel.DataAnnotations;

namespace PlanetHunters.DataImporter.DTOs
{
    public class PlanetDto
    {
        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; }

        [Range(0.001, double.MaxValue)]
        public double Mass { get; set; }

        [StringLength(255, MinimumLength = 1)]
        public string StarSystem { get; set; }
    }
}