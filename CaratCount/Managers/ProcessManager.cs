using CaratCount.Data;
using CaratCount.Entities;
using CaratCount.Interface;
using CaratCount.Migrations;
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
                .FirstOrDefaultAsync(p => p.Id == processId && p.UserId == userId);

            return process;
        }

        public async Task<List<Process>?> GetProcessesByUserIdAsync(string userId)
        {
            List<Process> processes = await _context.Processes
                .Where(p => p.UserId == userId)
                .ToListAsync();


            return processes;

        }

        public async Task AddProcessAsync(Process process)
        {
            try
            {
                _context.Processes.Add(process);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateProcessAsync(Process process)
        {
            Process? existingProcess = await _context.Processes.FindAsync(process.Id);

            if (existingProcess == null)
            {
                throw new ArgumentException("Process not found.");
            }

            try
            {
                existingProcess.Name = process.Name;
                existingProcess.Description = process.Description;

                _context.Processes.Update(existingProcess);
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
