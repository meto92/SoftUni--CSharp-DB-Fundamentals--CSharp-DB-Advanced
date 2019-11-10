using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Stations.DTOs.Import
{
    public class TicketTripDto
    {
        private const string DateFormat = "dd/MM/yyyy HH:mm";

        public string OriginStation { get; set; }

        public string DestinationStation { get; set; }

        [XmlIgnore]
        public DateTime DepartureTime { get; set; }

        [XmlElement(nameof(DepartureTime))]
        public string DepartureTimeStr
        {
            get => DepartureTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            set => DepartureTime = DateTime.ParseExact(value, DateFormat, CultureInfo.InvariantCulture);
        }
    }
}