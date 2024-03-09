using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CaratCount.Entities
{
    public class ProcessPrice
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User cost is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "User cost must be greater than 0")]
        public decimal UserCost { get; set; }

        [Required(ErrorMessage = "Client charge is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Client charge must be greater than 0")]
        public decimal ClientCharge { get; set; }

        public Guid ProcessId { get; set; }
       
        public  Process? Process { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
                                            