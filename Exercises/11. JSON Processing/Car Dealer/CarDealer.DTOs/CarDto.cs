using Newtonsoft.Json;

namespace CarDealer.DTOs
{
    public class CarDto
    {
        [JsonProperty("make")]
        public string Make { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("travelledDistance")]
        public long TravelledDistance { get; set; }
    }
}