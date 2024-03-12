using System.ComponentModel.DataAnnotations;

namespace CaratCount.Entities
{
    public enum ClarityLevel
    {
        FL,
        IF,
        VVS1,
        VVS2,
        VS1,
        VS2,
        SI1,
        SI2,
        I1,
        I2,
        I3
    }

    public enum CutType
    {
        Ideal,
        Excellent,
        VeryGood,
        Good,
        Fair,
        Poor
    }

    public enum ColorType
    {
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z
    }


    public class DiamondPacket
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Carat weight is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Carat weight must be greater than 0")]
        public decimal CaratWeight { get; set; }

        [Required(ErrorMessage = "Clarity is required")]
        public ClarityLevel Clarity { get; set; }

        [Required(ErrorMessage = "Cut type is required")]
        public CutType Cut { get; set; }

        [Required(ErrorMessage = "Color type is required")]
        public ColorType Color { get; set; }

        [Required(ErrorMessage = "Number of diamonds is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of diamonds must be greater than 0")]
        public int NumberOfDiamond { get; set; }

        [Required(ErrorMessage = "Receive date is required")]
        public DateTime? ReceiveDate { get; set; } = null;

        public DateTime? DeliveryDate { get; set; } = null;
        public Guid ClientId { get; set; }
        public string? UserId { get; set; }
        public Client? Client { get; set; }
        public ApplicationUser? User { get; set; }
        public ICollection<DiamondPacketProcess>? DiamondPacketProcesses { get; set; }


    }

}
