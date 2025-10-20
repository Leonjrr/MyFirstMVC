using System.Text.Json.Serialization;

namespace MyFirstMVC.Models
{
    public class TwoFactorResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("tokenType")]
        public string TokenType { get; set; }

        [JsonPropertyName("expiresOn")]
        public DateTime ExpiresOn { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("roleId")]
        public string RoleId { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("middleName")]
        public string MiddleName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("userType")]
        public string UserType { get; set; }

        [JsonPropertyName("userTypeId")]
        public int UserTypeId { get; set; }

        [JsonPropertyName("mdaName")]
        public string MdaName { get; set; }

        [JsonPropertyName("lgaName")]
        public string LgaName { get; set; }
    }
}
