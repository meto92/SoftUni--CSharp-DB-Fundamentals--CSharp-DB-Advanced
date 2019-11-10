using System.Xml.Serialization;

namespace ProductShop.DTOs
{
    [XmlType("user")]
    public class UserSoldProductsDto
    {
        [XmlAttribute("first-name")]
        public string FirstName { get; set; }

        [XmlAttribute("last-name")]
        public string LastName { get; set; }

        [XmlArray("sold-products")]
        public ProductDto[] SoldProducts { get; set; }
    }
}