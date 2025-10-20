namespace MyFirstMVC.Models
{
    public class LoginResponse
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public bool TwoFactorRequired { get; set; }
        public int ExpiresInMinutes { get; set; }
    }
}
