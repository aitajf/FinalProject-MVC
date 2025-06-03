using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Account
{
    public class UserPassword
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
