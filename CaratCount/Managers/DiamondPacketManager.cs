using CaratCount.Data;
using CaratCount.Entities;
using CaratCount.Interface;
using Microsoft.EntityFrameworkCore;

namespace CaratCount.Managers
{
    public class DiamondPacketManager : IDiamondPacketManager
    {
        private readonly ApplicationDbContext _context;

        public DiamondPacketManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DiamondPacket?> GetDiamondPacketByIdAsync(string id, string userId)
        {
            Guid diamondPacketId;

            if (!Guid.TryParse(id, out diamondPacketId) || string.IsNullOrEmpty(userId)) return null;


            DiamondPacket? diamondPacket = await _context.DiamondPackets
                .Include(d => d.Client)
                .Include(dp => dp.DiamondPacketProcesses)
                .FirstOrDefaultAsync(d => d.Id == diamondPacketId && d.UserId == userId);

            return diamondPacket;
        }

        public async Task<DiamondPacketProcess> GetDiamondPacketProcessByIdAsync(string id)
        {
            Guid diamondPacketProcessId;

            if (!Guid.TryParse(id, out diamondPacketProcessId)) return null;

            DiamondPacketProcess diamondPacketProcess = await _context.DiamondPacketProcesses
                        .Include(dpp => dpp.Employee)
                .Include(dpp => dpp.Process)
                .Include(dpp => dpp.ProcessPrice)
                .FirstOrDefaultAsync(dpp => dpp.Id == diamondPacketProcessId);

            return diamondPacketProcess;
        }

        public async Task<List<DiamondPacketProcess>?> GetDiamondPacketProcessesByDiamondPacketIdAsync(string id)
        {
            Guid diamondPacketId;

            if (!Guid.TryParse(id, out diamondPacketId)) return null;

            List<DiamondPacketProcess> diamondPacketProcesses = await _context.DiamondPacketProcesses
                        .Include(dpp => dpp.Employee)
                .Include(dpp => dpp.Process)
                .Include(dpp => dpp.ProcessPrice)
                .Where(dpp => dpp.DiamondPacketId == diamondPacketId)
                .ToListAsync();

            return diamondPacketProcesses;
        }
        public async Task<List<DiamondPacketProcess>?> GetDiamondPacketProcessesByEmployeeByIdAsync(string id)
        {
            Guid employeeId;

            if (!Guid.TryParse(id, out employeeId)) return null;

            List<DiamondPacketProcess> diamondPacketProcesses = await _context.DiamondPacketProcesses
                        .Include(dpp => dpp.Employee)
                .Include(dpp => dpp.Process)
                .Include(dpp => dpp.ProcessPrice)
                .Where(dpp => dpp.EmployeeId == employeeId)
                .ToListAsync();

            return diamondPacketProcesses;
        }
        public async Task<List<DiamondPacket>?> GetDiamondPacketsByUserIdAsync(string userId)
        {
            List<DiamondPacket> diamondPackets = await _context.DiamondPackets
                .Include(d => d.Client)
                .Where(d => d.UserId == userId)
                .ToListAsync();

            return diamondPackets;
        }

        public async Task<List<DiamondPacket>?> GetDiamondPacketsByClientIdAsync(string id)
        {
            Guid clientId;

            if (!Guid.TryParse(id, out clientId)) return null;

            List<DiamondPacket> diamondPackets = await _context.DiamondPackets
                .Where(d => d.ClientId == clientId)
                .ToListAsync();

            return diamondPackets;
        }

        public async Task AddDiamondPacketAsync(DiamondPacket diamondPacket)
        {
            try
            {
                _context.DiamondPackets.Add(diamondPacket);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateDiamondPacketAsync(DiamondPacket diamondPacket)
        {
            DiamondPacket? existingDiamondPacket = await _context.DiamondPackets.FindAsync(diamondPacket.Id);

            if (existingDiamondPacket == null)
            {
                throw new ArgumentException("Diamond packet not found.");
            }

            try
            {
                // Update the properties of the existing diamond packet
                existingDiamondPacket.CaratWeight = diamondPacket.CaratWeight;
                existingDiamondPacket.Clarity = diamondPacket.Clarity;
                existingDiamondPacket.Cut = diamondPacket.Cut;
                existingDiamondPacket.Color = diamondPacket.Color;
                existingDiamondPacket.NumberOfDiamond = diamondPacket.NumberOfDiamond;
                existingDiamondPacket.ReceiveDate = diamondPacket.ReceiveDate;
                existingDiamondPacket.DeliveryDate = diamondPacket.DeliveryDate;
                existingDiamondPacket.ClientId = diamondPacket.ClientId;

                _context.DiamondPackets.Update(existingDiamondPacket);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task DeleteDiamondPacketAsync(DiamondPacket diamondPacket)
        {
            try
            {
                if (diamondPacket == null)
                {
                    throw new ArgumentNullException(nameof(diamondPacket));
                }

                _context.DiamondPackets.Remove(diamondPacket);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AssignDiamondPacketProcessAsync(DiamondPacketProcess diamondPacketProcess)
        {
            try
            {
                if (diamondPacketProcess == null)
                {
                    throw new ArgumentNullException(nameof(diamondPacketProcess));
                }

                _context.DiamondPacketProcesses.Add(diamondPacketProcess);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateDiamondPacketProcessAsync(DiamondPacketProcess diamondPacketProcess)
        {
            try
            {
                if (diamondPacketProcess == null)
                {
                    throw new ArgumentNullException(nameof(diamondPacketProcess));
                }

                _context.DiamondPacketProcesses.Update(diamondPacketProcess);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
