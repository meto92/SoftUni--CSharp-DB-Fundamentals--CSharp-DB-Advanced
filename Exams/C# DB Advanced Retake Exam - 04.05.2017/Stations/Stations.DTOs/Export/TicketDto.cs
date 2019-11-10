using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Stations.DTOs.Export
{
    public class TicketDto
    {
        private const string DateFormat = "dd/MM/yyyy HH:mm";

        public string OriginStation { get; set; }

        public string DestinationStation { get; set; }

        [XmlIgnore]
        public DateTime DepartureTime { get; set; }

        [XmlElement(nameof(DepartureTime))]
        public string DepartureTimeStr => DepartureTime.ToString(DateFormat, CultureInfo.InvariantCulture);
    }
}