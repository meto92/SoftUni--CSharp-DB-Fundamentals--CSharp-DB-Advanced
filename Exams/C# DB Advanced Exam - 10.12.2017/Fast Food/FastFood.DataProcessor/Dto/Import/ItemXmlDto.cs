using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace FastFood.DataProcessor.Dto.Import
{
    [XmlType("Item")]
    public class ItemXmlDto
    {
        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}