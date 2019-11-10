using System.ComponentModel.DataAnnotations;

namespace PlanetHunters.DataImporter.DTOs
{
    public class TelescopeDto
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Location { get; set; }

        [Range(0.001, 1234)]
        public double? MirrorDiameter { get; set; }
    }
}