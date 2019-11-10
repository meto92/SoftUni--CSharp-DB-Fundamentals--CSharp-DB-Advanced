using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

using Newtonsoft.Json;

namespace Stations.DTOs.Import
{
    public class TripDto
    {
        private const string DateFormat = "dd/MM/yyyy HH:mm";

        [Required]                
        public string Train { get; set; }

        public string Status { get; set; }

        [Required]
        public string OriginStation { get; set; }

        [Required]
        public string DestinationStation { get; set; }

        [Required]
        [JsonProperty(nameof(DepartureTime))]
        public string DepartureTimeStr { get; set; }

        [Required]
        [JsonProperty(nameof(ArrivalTime))]
        public string ArrivalTimeStr { get; set; }

        public string TimeDifference { get; set; }

        [JsonIgnore]
        public DateTime DepartureTime => DateTime.ParseExact(DepartureTimeStr, DateFormat, CultureInfo.InvariantCulture);

        [JsonIgnore]
        public DateTime ArrivalTime => DateTime.ParseExact(ArrivalTimeStr, DateFormat, CultureInfo.InvariantCulture);
    }
}