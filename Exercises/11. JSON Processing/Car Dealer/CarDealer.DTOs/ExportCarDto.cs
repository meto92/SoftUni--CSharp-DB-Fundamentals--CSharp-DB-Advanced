using Newtonsoft.Json;

namespace CarDealer.DTOs
{
    public class ExportCarDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }
    }
}