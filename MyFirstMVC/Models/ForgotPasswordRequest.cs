using System.ComponentModel.DataAnnotations;

namespace MyFirstMVC.Models
{
    public class ForgotPasswordRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmNewPassword { get; set; }
    }
}
