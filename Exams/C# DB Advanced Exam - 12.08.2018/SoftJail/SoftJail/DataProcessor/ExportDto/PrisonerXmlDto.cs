using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class PrisonerXmlDto
    {
        public int Id { get; set; }

        [XmlElement("Name")]
        public string FullName { get; set; }

        public string IncarcerationDate { get; set; }

        [XmlArrayItem("Message")]
        public MesagedDto[] EncryptedMessages { get; set; }
    }
}