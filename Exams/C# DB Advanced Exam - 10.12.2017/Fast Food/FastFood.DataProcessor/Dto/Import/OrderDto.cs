using System;
using System.Globalization;
using System.Xml.Serialization;

namespace FastFood.DataProcessor.Dto.Import
{
    [XmlType("Order")]
    public class OrderDto
    {
        private const string DateFormat = "dd/MM/yyyy HH:mm";

        public string Customer { get; set; }

        public string Employee { get; set; }

        [XmlIgnore]
        public DateTime DateTime { get; set; }

        [XmlElement(nameof(DateTime))]
        public string Date
        {
            get => this.DateTime.ToString(DateFormat, CultureInfo.InvariantCulture);
            set => this.DateTime = DateTime.ParseExact(value, DateFormat, CultureInfo.InvariantCulture);
        }

        public string Type { get; set; }

        [XmlArray("Items")]
        public ItemXmlDto[] Items { get; set; }
    }
}