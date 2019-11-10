using System.ComponentModel.DataAnnotations;

namespace PetClinic.DTOs
{
    public class AnimalDto
    {
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string Type { get; set; }

        [Range(1, 333)]
        public int Age { get; set; }
        
        [Required]
        public PassportDto Passport { get; set; }
    }
}