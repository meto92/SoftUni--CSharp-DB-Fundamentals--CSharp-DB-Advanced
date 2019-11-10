using System.Xml.Serialization;

namespace ProductShop.DTOs
{
    [XmlType("category")]
    public class CategoryWithProductsInfoDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("products-count")]
        public int ProductsCount { get; set; }

        [XmlElement("average-price")]
        public decimal? AveragePrice { get; set; }

        [XmlElement("total-revenue")]
        public decimal TotalRevenue { get; set; }
    }
}