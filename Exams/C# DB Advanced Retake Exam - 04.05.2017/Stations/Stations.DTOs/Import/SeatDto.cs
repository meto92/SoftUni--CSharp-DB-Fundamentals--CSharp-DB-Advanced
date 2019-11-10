using System.ComponentModel.DataAnnotations;

namespace Stations.DTOs.Import
{
    public class SeatDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string Abbreviation { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? Quantity { get; set; }
    }
}