using System.Xml.Serialization;

namespace CarDealer.DTOs
{
    [XmlType("sale")]
    public class SaleDto
    {
        [XmlElement("car")]
        public CarDto Car { get; set; }

        [XmlElement("customer-name")]
        public string CustomerName { get; set; }

        [XmlIgnore]
        public bool IsCustomerYoungDriver { get; set; }

        [XmlElement("discount")]
        public double Discount { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("price-with-discount")]
        public decimal PriceWithDiscount { get; set; }
    }
}