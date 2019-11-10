using System.Xml.Serialization;

namespace CarDealer.DTOs
{
    [XmlType("car")]
    public class CarDto
    {
        [XmlIgnore]
        public int Id { get; set; }

        [XmlAttribute("make")]
        //[XmlElement("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        //[XmlElement("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        //[XmlElement("travelled-distance")]
        public long TravelledDistance { get; set; }
    }
}