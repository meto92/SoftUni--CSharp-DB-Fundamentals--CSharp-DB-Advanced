using System.Xml.Serialization;

namespace ProductShop.DTOs
{
    [XmlType("sold-products")]
    public class SoldProductsDto
    {
        [XmlAttribute("count")]
        public int ProductsCount { get; set; }

        [XmlElement("product")]
        public ProductDto[] Products { get; set; }
    }
}