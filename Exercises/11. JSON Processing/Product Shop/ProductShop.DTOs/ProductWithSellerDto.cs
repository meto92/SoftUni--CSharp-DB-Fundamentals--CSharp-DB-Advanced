﻿using Newtonsoft.Json;

namespace ProductShop.DTOs
{
    public class ProductWithSellerDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("seller")]
        public string Seller { get; set; }
    }
}