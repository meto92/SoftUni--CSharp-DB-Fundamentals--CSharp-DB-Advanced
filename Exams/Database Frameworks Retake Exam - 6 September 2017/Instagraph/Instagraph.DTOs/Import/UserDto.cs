using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace Instagraph.DTOs.Import
{
    public class UserDto
    {
        [Required]
        [JsonProperty("username")]
        public string Username { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }

        [Required]
        [JsonProperty("profile_picture")]
        public string ProflePicture { get; set; }
    }
}