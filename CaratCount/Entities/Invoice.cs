using System.ComponentModel.DataAnnotations;

namespace CaratCount.Entities
{
    public enum InvoiceStatus
    {
        Paid,
        Unpaid
    }
    public class Invoice
    {
        public Guid Id { get; set; }

        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Description status is required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Payment status is required")]
        public InvoiceStatus PaymentStatus { get; set; }    = InvoiceStatus.Unpaid;

        [Required(ErrorMessage = "Issue date is required")]
        public DateTime? IssueDate { get; set; } = null;

        [Required(ErrorMessage = "Due date is required")]
        public DateTime? DueDate { get; set; } = null;

        [Required(ErrorMessage = "Client is required")]
        public Guid ClientId { get; set; }

        public Client? Client { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public ICollection<InvoiceItem>? InvoiceItems { get; set; }

    }
}
