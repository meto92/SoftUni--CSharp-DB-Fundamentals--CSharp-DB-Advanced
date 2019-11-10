using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Stations.DTOs.Import
{
    [XmlType("Card")]
    public class CardDto
    {
        [StringLength(128)]
        public string Name { get; set; }

        [Range(0, 120)]
        public int Age { get; set; }

        public string CardType { get; set; }
    }
}