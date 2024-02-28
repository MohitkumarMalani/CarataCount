using System.ComponentModel.DataAnnotations;

namespace CaratCount.Models
{
    public class ClientViewModel
    {
        public string UserId { get; set; }
        public Guid ClientId { get; set; } = Guid.NewGuid();
        public Guid GstInDetailId { get; set; } = Guid.NewGuid();
        public Guid AddressId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 digits")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "GSTIN is required")]
        [RegularExpression(@"^\d{2}[A-Z]{5}\d{4}[A-Z]\d{1}[A-Z]\d{1}$", ErrorMessage = "GSTIN must be in the format 00AAAAA0000A0Z0")]
        public string? GstInNo { get; set; }

        [Required(ErrorMessage = "Legal name is required")]
        public string? LegalName { get; set; }

        [Required(ErrorMessage = "Trade name is required")]
        public string? TradeName { get; set; }
        [Required(ErrorMessage = "Building Name is required")]
        public string? BuildingName { get; set; }

        [Required(ErrorMessage = "Street Name is required")]
        public string? StreetName { get; set; }

        [Required(ErrorMessage = "Floor Number is required")]
        public string? FloorNumber { get; set; }
        public string? UnitNumber { get; set; }

        [Required(ErrorMessage = "Locality is required")]
        public string? Locality { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }

        [Required(ErrorMessage = "District is required")]
        public string? District { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "PostalCode must be a six-digit number.")]
        public string? PostalCode { get; set; }
    }
}
