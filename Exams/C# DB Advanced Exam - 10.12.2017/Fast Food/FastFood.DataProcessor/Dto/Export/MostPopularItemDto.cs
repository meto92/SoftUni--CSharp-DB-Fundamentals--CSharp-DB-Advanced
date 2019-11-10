using System.Xml.Serialization;

namespace FastFood.DataProcessor.Dto.Export
{
    public class MostPopularItemDto
    {
        public string Name { get; set; }

        [XmlIgnore]
        public decimal TotalMade { get; set; }

        [XmlElement(nameof(TotalMade))]
        public string TotalMadeStr
        {
            get => this.TotalMade.ToString("F2");
            set => this.TotalMade = decimal.Parse(value);
        }

        public int TimesSold { get; set; }
    }
}