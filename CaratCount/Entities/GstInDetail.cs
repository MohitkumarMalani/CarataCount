using System.ComponentModel.DataAnnotations;
using System.Net;

namespace CaratCount.Entities
{
    public class GstInDetail
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "GSTIN is required")]
        [RegularExpression(@"^\d{2}[A-Z]{5}\d{4}[A-Z]\d{1}$", ErrorMessage = "GSTIN must be in the format 00AAAAA0000A0Z0")]
        public string? GstInNo { get; set; }
        public string UserId { get; set; }

        [Required(ErrorMessage = "Legal name is required")]
        public string? LegalName { get; set; }

        [Required(ErrorMessage = "Trade name is required")]
        public string? TradeName { get; set; }
      

        // Navigation properties
        public ApplicationUser? User { get; set; }
  
    }
}
