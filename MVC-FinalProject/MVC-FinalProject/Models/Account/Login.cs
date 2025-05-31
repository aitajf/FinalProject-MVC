using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.Account
{
    public class Login
    {
        [Required]
        public string EmailOrUserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
