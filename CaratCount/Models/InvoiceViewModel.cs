using CaratCount.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CaratCount.Models
{
    public class InvoiceViewModel
    {
        public string? UserId { get; set; }

        public Guid InvoiceId { get; set; }

        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Description status is required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Payment status is required")]
        public InvoiceStatus PaymentStatus { get; set; } = InvoiceStatus.Unpaid;

        [Required(ErrorMessage = "Issue date is required")]
        public DateTime? IssueDate { get; set; } = null;

        [Required(ErrorMessage = "Due date is required")]
        public DateTime? DueDate { get; set; } = null;

        [Required(ErrorMessage = "Client is required")]
        public Guid ClientId { get; set; }

       public List<Client>? Clients { get; set; }


        public MultiSelectList? DiamondPackets { get; set; }

        public List<Guid>? DiamondPacketIds { get; set; }
    }

   

}
