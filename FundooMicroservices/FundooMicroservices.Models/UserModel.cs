using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FundooMicroservices.Models
{
    public class UserModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [RegularExpression(@"^[A-Z][a-z]{2,15}$", ErrorMessage = "Invalid Format")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("date")]
        public DateTime RegisterDate { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z0-9]{1,25}@[A-Za-z0-9]{2,25}[.][a-z]{2,5}([.][a-z]{2,5})?$", ErrorMessage = "Invalid Format")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z0-9]{8,15}$", ErrorMessage = "Invalid Format")]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}