using Newtonsoft.Json;

namespace CarDealer.DTOs
{
    public class SupplierDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isImporter")]
        public bool IsImporter { get; set; }
    }
}