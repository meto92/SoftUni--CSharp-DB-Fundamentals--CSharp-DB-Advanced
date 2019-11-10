using System.Xml.Serialization;

namespace Instagraph.DTOs.Import
{
    public class CommentPostDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}