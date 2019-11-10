using System.Xml.Serialization;

namespace ProductShop.DTOs
{
    [XmlRoot("users")]
    public class SellersDto
    {
        [XmlAttribute("count")]
        public int SellersCount { get; set; }

        [XmlElement("user")]
        public UserAndProductsDto[] Sellers { get; set; }
    }
}