using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace Instagraph.DTOs.Import
{
    public class PictureDto
    {
        [Required]
        [JsonProperty("path")]
        public string Path { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [JsonProperty("size")]
        public decimal Size { get; set; }
    }
}