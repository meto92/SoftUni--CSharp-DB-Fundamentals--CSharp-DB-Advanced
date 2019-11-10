using Newtonsoft.Json;

namespace CarDealer.DTOs
{
    public class CarWithPartsDto
    {
        [JsonProperty("car")]
        public ExportCarDto Car { get; set; }

        [JsonProperty("parts")]
        public CarPartDto[] Parts { get; set; }
    }
}