using Newtonsoft.Json;

namespace CarDealer.DTOs
{
    public class SaleDto
    {
        [JsonProperty("car")]
        public ExportCarDto Car { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonIgnore]
        public bool IsCustomerYoungDriver { get; set; }

        [JsonProperty("Discount")]
        public double Discount { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("priceWithDiscount")]
        public decimal PriceWithDiscount { get; set; }
    }
}