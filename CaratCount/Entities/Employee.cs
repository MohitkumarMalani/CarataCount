using System.ComponentModel.DataAnnotations;

namespace CaratCount.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number")] 
        public string? PhoneNumber { get; set; }

        public string? UserId { get; set; }

        public ApplicationUser? User { get; set; }

    }
}
