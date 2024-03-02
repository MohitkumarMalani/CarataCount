using CaratCount.Entities;

namespace CaratCount.Interface
{
    public interface IDiamondPacketManager
    {
        Task<DiamondPacket?> GetDiamondPacketByIdAsync(string id, string userId);

        Task<List<DiamondPacket>?> GetDiamondPacketsByUserIdAsync(string userId);

        Task AddDiamondPacketAsync(DiamondPacket diamondPacket);

        Task UpdateDiamondPacketAsync(DiamondPacket diamondPacket);

        Task DeleteDiamondPacketAsync(DiamondPacket diamondPacket);
    }
}
