using System.ComponentModel.DataAnnotations;

namespace CaratCount.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter a user name.")]
        [StringLength(255)]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [StringLength(255)]
        public string? Password { get; set; }

        public string? ReturnUrl { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
