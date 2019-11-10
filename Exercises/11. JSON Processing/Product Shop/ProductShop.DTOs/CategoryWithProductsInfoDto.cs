﻿using Newtonsoft.Json;

namespace ProductShop.DTOs
{
    public class CategoryWithProductsInfoDto
    {
        [JsonProperty("category")]
        public string Name { get; set; }

        [JsonProperty("productsCount")]
        public int ProductsCount { get; set; }

        [JsonProperty("averagePrice")]
        public decimal? AveragePrice { get; set; }

        [JsonProperty("totalRevenue")]
        public decimal TotalRevenue { get; set; }
    }
}