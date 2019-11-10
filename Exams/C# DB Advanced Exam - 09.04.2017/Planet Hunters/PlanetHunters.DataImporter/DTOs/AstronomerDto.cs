using System.ComponentModel.DataAnnotations;

namespace PlanetHunters.DataImporter.DTOs
{
    public class AstronomerDto
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }
    }
}