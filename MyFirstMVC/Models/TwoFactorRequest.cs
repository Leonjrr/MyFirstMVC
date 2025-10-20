using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyFirstMVC.Models
{
    public class TwoFactorRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required]
        [JsonPropertyName("otpCode")]
        public int OtpCode { get; set; }

        [JsonPropertyName("isPasswordReset")]
        public bool IsPasswordReset { get; set; } = false; //Default to false for login flow
    }
}
