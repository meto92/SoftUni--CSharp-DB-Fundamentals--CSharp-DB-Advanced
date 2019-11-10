using Newtonsoft.Json;

namespace PetClinic.DTOs
{
    public class ExportAnimalDto
    {
        public string OwnerName { get; set; }

        [JsonProperty("AnimalName")]
        public string Name { get; set; }

        public int Age { get; set; }

        [JsonProperty("SerialNumber")]
        public string PassportSerialNumber { get; set; }

        public string RegisteredOn { get; set; }
    }
}