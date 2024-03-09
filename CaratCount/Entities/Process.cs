using CaratCount.Migrations;
using System.ComponentModel.DataAnnotations;

namespace CaratCount.Entities
{
    public class Process
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Process Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Process Description is required")]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<ProcessPrice>? ProcessPrices { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
