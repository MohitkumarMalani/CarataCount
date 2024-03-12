using System.ComponentModel.DataAnnotations;
using CaratCount.Migrations;

namespace CaratCount.Entities
{
    public enum ProcessStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }
    public class DiamondPacketProcess
    {
        public Guid Id { get; set; } 
        public decimal FinalCaratWeight { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime? StartDate { get; set; } = null;
        public DateTime? CompleteDate { get; set; } = null;
        public ProcessStatus Status { get; set; }  = ProcessStatus.Pending;

        [Required(ErrorMessage = "Diamond Packet Id is required")]
        public Guid? DiamondPacketId { get; set; }
        public DiamondPacket? DiamondPacket { get; set; }

        [Required(ErrorMessage = "Employee is required")]

        public Guid EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        [Required(ErrorMessage = "Process is required")]
        public Guid ProcessId { get; set; }
        public Process? Process { get; set; }

        public Guid ProcessPriceId { get; set; }
        public ProcessPrice? ProcessPrice { get; set; }

    }
}
