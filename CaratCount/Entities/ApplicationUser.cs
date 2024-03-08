using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CaratCount.Entities
{

    public class ApplicationUser : IdentityUser
    {

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number")]
        public override string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "GSTIN is required")]
        [RegularExpression(@"^\d{2}[A-Z]{5}\d{4}[A-Z]\d{1}[A-Z]\d{1}$$", ErrorMessage = "GSTIN must be in the format 00AAAAA0000A0Z0")]
        public string? GstInNo { get; set; }

        public bool IsBlocked { get; set; } = false;

        public Guid? GstInDetailId { get; set; } = null;
        public GstInDetail? GstInDetail { get; set; }
        public ICollection<Client>? Clients { get; set; }
        public ICollection<Employee>? Employees { get; set; }
        public ICollection<Process>? Processes { get; set; }

    }
}
