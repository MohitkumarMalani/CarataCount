using System.ComponentModel.DataAnnotations;

namespace CaratCount.Entities
{
    public class InvoiceItem
    {
        public Guid Id { get; set; }

        public decimal UserCost { get; set; }

        public decimal ClientCharge { get; set; }

        public Guid DiamondPacketId { get; set; }
        public DiamondPacket? DiamondPacket { get; set; }

        public Guid InvoiceId { get; set; }
        public Invoice? Invoice {  get; set; }

    }
}
