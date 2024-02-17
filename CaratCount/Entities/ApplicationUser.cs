using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CaratCount.Entities
{

    public class ApplicationUser : IdentityUser
    {

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public override string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "GSTIN is required")]
        [RegularExpression(@"^\d{2}[A-Z]{5}\d{4}[A-Z]\d{1}$", ErrorMessage = "GSTIN must be in the format 00AAAAA0000A0Z0")]
        public string? GstInNo { get; set; }

        public ICollection<GstInDetail>? GstInDetails { get; set; }
    }
}
