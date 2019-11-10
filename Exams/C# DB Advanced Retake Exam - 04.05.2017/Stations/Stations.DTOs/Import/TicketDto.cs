using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Stations.DTOs.Import
{
    [XmlType("Ticket")]
    public class TicketDto
    {
        [XmlAttribute("price")]
        [Range(typeof(decimal), "0.01m", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [XmlAttribute("seat")]
        public string Seat { get; set; }

        public TicketTripDto Trip { get; set; }

        public TicketCardDto Card { get; set; }
    }
}