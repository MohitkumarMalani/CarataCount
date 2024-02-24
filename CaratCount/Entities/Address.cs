using System.ComponentModel.DataAnnotations;
using CaratCount.Migrations;

namespace CaratCount.Entities
{
    public class Address
    {
        public Guid Id { get; set; }

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

        public GstInDetail? GstInDetail { get; set; }
    }
}
