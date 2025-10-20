using System.Text.Json.Serialization;

namespace MyFirstMVC.Models
{
    public class ApiResponseWrapper <T>
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("isSuccessful")]
        public bool IsSuccessful { get; set; }

        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
