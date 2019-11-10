using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Instagraph.DTOs.Import
{
    [XmlType("comment")]
    public class CommentDto
    {
        [Required]
        [XmlElement("content")]
        public string Content { get; set; }

        [Required]
        [XmlElement("user")]
        public string User { get; set; }

        [Required]
        [XmlElement("post")]
        public CommentPostDto Post { get; set; }
    }
}