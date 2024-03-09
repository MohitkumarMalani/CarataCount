using System.ComponentModel.DataAnnotations;

namespace CaratCount.Models
{
    public class ProcessViewModel
    {
        public string UserId { get; set; }

        public Guid ProcessId { get; set; }

        [Required(ErrorMessage = "Process Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Process Description is required")]
        public string? Description { get; set; }


        [Required(ErrorMessage = "User cost is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "User cost must be greater than 0")]
        public decimal UserCost { get; set; }

        [Required(ErrorMessage = "Client charge is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Client charge must be greater than 0")]
        public decimal ClientCharge { get; set; }
    }
}
