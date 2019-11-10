using System.ComponentModel.DataAnnotations;

namespace Stations.DTOs.Import
{
    public class SeatingClassDto
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string Abbreviation { get; set; }
    }
}