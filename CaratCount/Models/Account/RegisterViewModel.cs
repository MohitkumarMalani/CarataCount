using System.ComponentModel.DataAnnotations;

namespace CaratCount.Models.Account
{
    public class RegisterViewModel 
    {
        [Required(ErrorMessage = "User name is required")]
        [StringLength(255)]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "User email is required")]
        [StringLength(255)]
        [EmailAddress(ErrorMessage ="Invalid email address")]
        public string? UserEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 digits")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "GSTIN is required")]
        [RegularExpression(@"^\d{2}[A-Z]{5}\d{4}[A-Z]\d{1}[A-Z]\d{1}$", ErrorMessage = "GSTIN must be in the format 00AAAAA0000A0Z0")]
        public string? GstInNo { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [StringLength(255)]
        public string? Password { get; set; }

    }
}
