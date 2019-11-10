using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

using Newtonsoft.Json;

namespace SoftJail.DataProcessor.ImportDto
{
    public class PrisonerDto
    {
        private const string DateFormat = "dd/MM/yyyy"; 

        [Required]
        [MinLength(3), MaxLength(20)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression("^The [A-Z][a-z]*$")]
        public string Nickname { get; set; }

        [Range(18, 65)]
        public int Age { get; set; }

        [JsonIgnore]
        public DateTime IncarcerationDate { get; set; }

        [JsonProperty(nameof(IncarcerationDate))]
        public string IncarcerationDateStr
        {
            get => IncarcerationDate.ToString();
            set => IncarcerationDate = DateTime.ParseExact(value, DateFormat, CultureInfo.InvariantCulture);
        }
        
        public string ReleaseDate { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal? Bail { get; set; }

        public int CellId { get; set; }

        public MailDto[] Mails { get; set; }
    }
}