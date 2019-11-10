using System.Xml.Serialization;

namespace Instagraph.DTOs.Export
{
    [XmlType("user")]
    public class Dto
    {
        [XmlElement("username")]
        public string Username { get; set; }

        [XmlElement("most-comments")]
        public int MostComments { get; set; }
    }
}