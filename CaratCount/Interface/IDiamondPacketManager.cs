using CaratCount.Entities;

namespace CaratCount.Interface
{
    public interface IDiamondPacketManager
    {
        Task<DiamondPacket?> GetDiamondPacketByIdAsync(string id, string userId);
        Task<DiamondPacketProcess> GetDiamondPacketProcessByIdAsync(string id);

        Task<List<DiamondPacket>?> GetDiamondPacketsByUserIdAsync(string userId);
        Task<List<DiamondPacketProcess>?> GetDiamondPacketProcessesByDiamondPacketIdAsync(string id);

        Task AddDiamondPacketAsync(DiamondPacket diamondPacket);

        Task UpdateDiamondPacketAsync(DiamondPacket diamondPacket);

        Task DeleteDiamondPacketAsync(DiamondPacket diamondPacket);

        Task AssignDiamondPacketProcessAsync(DiamondPacketProcess diamondPacketProcess);

        Task UpdateDiamondPacketProcessAsync(DiamondPacketProcess diamondPacketProcess);
    }
}
