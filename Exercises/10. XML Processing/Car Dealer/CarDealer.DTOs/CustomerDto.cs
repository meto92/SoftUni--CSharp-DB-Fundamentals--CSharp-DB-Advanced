using System;
using System.Xml.Serialization;

namespace CarDealer.DTOs
{
    [XmlType("customer")]
    public class CustomerDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("birth-date")]
        public DateTime BirthDate { get; set; }

        [XmlElement("is-young-driver")]
        public Boolean IsYoungDriver { get; set; }
    }
}