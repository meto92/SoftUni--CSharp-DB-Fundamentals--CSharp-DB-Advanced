using Newtonsoft.Json;

namespace PlanetHunters.DataExporter.DTOs
{
    public class AstronomerDto
    {
        [JsonIgnore]
        public string FirstName { get; set; }

        [JsonIgnore]
        public string LastName { get; set; }

        [JsonProperty("Name")]
        public string FullName => this.FirstName + " " + this.LastName;

        public string Role { get; set; }
    }
}