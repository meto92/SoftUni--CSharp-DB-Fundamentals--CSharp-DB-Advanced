using System.ComponentModel.DataAnnotations;

namespace Stations.DTOs.Import
{
    public class StationDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Town { get; set; }
    }
}