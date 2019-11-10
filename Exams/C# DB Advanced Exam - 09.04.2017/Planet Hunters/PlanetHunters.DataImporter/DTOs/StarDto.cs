using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace PlanetHunters.DataImporter.DTOs
{
    [XmlType("Star")]
    public class StarDto
    {
        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; }

        [Range(2400, int.MaxValue)]
        public int Temperature { get; set; }

        [MinLength(1)]
        public string StarSystem { get; set; }
    }
}