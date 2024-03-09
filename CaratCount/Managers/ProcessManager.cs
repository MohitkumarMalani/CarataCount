using CaratCount.Data;
using CaratCount.Entities;
using CaratCount.Interface;
using CaratCount.Models;
using Microsoft.EntityFrameworkCore;

namespace CaratCount.Managers
{
    public class ProcessManager : IProcessManager
    {
        private readonly ApplicationDbContext _context;

        public ProcessManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Process?> GetProcessByIdAsync(string id, string userId)
        {

            Guid processId;

            if (!Guid.TryParse(id, out processId) || string.IsNullOrEmpty(userId)) return null;

            Process? process = await _context.Processes
               .Include(p => p.ProcessPrices)
               .Where(p => p.Id == processId && p.UserId == userId)
               .FirstOrDefaultAsync();

            
            return process;
        }

        public async Task<List<Process>?> GetProcessesByUserIdAsync(string userId)
        {
            List<Process> processes = await _context.Processes
             .Include(p => p.ProcessPrices)
             .Where(p => p.UserId == userId)
             .OrderByDescending(p => p.ProcessPrices.Max(pp => pp.UpdatedAt))
             .ToListAsync();

            return processes;

        }

        public async Task AddProcessAsync(ProcessViewModel processViewModel)
        {
            try
            {


                Process? process = new()
                {
                    Name = processViewModel.Name,
                    Description = processViewModel.Description,
                    UserId = processViewModel.UserId
                };

                _context.Processes.Add(process);
                await _context.SaveChangesAsync();

                ProcessPrice? processPrice = new()
                {
                    UserCost = processViewModel.UserCost,
                    ClientCharge = processViewModel.ClientCharge,
                    ProcessId = process.Id
                };

                _context.ProcessPrices.Add(processPrice);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateProcessAsync(ProcessViewModel processViewModel)
        {
            Process? process = await _context.Processes.Include(p => p.ProcessPrices)
               .Where(p => p.Id == processViewModel.ProcessId && p.UserId == processViewModel.UserId)
               .OrderByDescending(p => p.ProcessPrices.Max(pp => pp.UpdatedAt))
               .FirstOrDefaultAsync();

            if (process == null)
            {
                throw new ArgumentException("Process not found.");
            }

            try
            {
                process.Name = processViewModel.Name;
                process.Description = processViewModel.Description;

                _context.Processes.Update(process);

                if (process?.ProcessPrices?.Reverse().FirstOrDefault()?.UserCost != processViewModel.UserCost ||
                    process?.ProcessPrices?.Reverse().FirstOrDefault()?.ClientCharge != processViewModel.ClientCharge)
                {

                    ProcessPrice? newProcessPrice = new()
                    {
                        UserCost = processViewModel.UserCost,
                        ClientCharge = processViewModel.ClientCharge,
                        ProcessId = process.Id
                    };

                    _context.ProcessPrices.Add(newProcessPrice);
                }

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteProcessAsync(Process process)
        {
            try
            {
                if (process == null)
                {
                    throw new ArgumentNullException(nameof(process));
                }

                _context.Processes.Remove(process);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
