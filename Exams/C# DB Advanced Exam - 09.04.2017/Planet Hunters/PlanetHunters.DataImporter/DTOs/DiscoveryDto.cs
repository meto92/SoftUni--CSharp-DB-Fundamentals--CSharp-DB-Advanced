using System;
using System.Globalization;
using System.Xml.Serialization;

namespace PlanetHunters.DataImporter.DTOs
{
    [XmlType("Discovery")]
    public class DiscoveryDto
    {
        private const string DateFormat = "yyyy-MM-dd";

        [XmlAttribute(nameof(DateMade))]
        public string Date
        {
            get => DateMade.ToString(DateFormat);
            set => DateMade = DateTime.ParseExact(value, DateFormat, CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public DateTime DateMade { get; set; }

        [XmlAttribute]
        public string Telescope { get; set; }

        [XmlArrayItem("Star")]
        public DiscoveryStarDto[] Stars { get; set; }

        [XmlArrayItem("Planet")]
        public DiscoveryPlanetDto[] Planets { get; set; }

        [XmlArrayItem("Astronomer")]
        public PioneerDto[] Pioneers { get; set; }

        [XmlArrayItem("Astronomer")]
        public ObserverDto[] Observers { get; set; }
    }
}