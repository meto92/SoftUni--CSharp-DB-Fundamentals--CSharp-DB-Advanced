using Newtonsoft.Json;

namespace CarDealer.DTOs
{
    public class CustomerWithTotalSalesDto
    {
        [JsonProperty("fullName")]
        public string Name { get; set; }

        [JsonProperty("boughtCars")]
        public int BoughtCars { get; set; }

        [JsonProperty("spentMoney")]
        public decimal SpentMoney { get; set; }
    }
}