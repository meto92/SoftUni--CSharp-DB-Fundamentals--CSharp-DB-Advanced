using System.Xml.Serialization;

namespace Stations.DTOs.Import
{
    public class TicketCardDto
    {
        [XmlAttribute]
        public string Name { get; set; }
    }
}