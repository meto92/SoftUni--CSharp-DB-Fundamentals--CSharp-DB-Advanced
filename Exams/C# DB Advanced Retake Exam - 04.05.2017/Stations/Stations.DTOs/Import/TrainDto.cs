using System.ComponentModel.DataAnnotations;

namespace Stations.DTOs.Import
{
    public class TrainDto
    {
        [Required]
        [StringLength(10)]
        public string TrainNumber { get; set; }

        public string Type { get; set; }

        public SeatDto[] Seats { get; set; }
    }
}