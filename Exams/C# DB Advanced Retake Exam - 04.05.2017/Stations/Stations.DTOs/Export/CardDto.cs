using System.Xml.Serialization;

namespace Stations.DTOs.Export
{
    [XmlType("Card")]
    public class CardDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlArrayItem("Ticket")]
        public TicketDto[] Tickets { get; set; }
    }
}