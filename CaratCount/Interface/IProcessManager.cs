using CaratCount.Entities;
using CaratCount.Models;

namespace CaratCount.Interface
{
    public interface IProcessManager
    {
        Task<Process?> GetProcessByIdAsync(string id, string userId);

        Task<List<Process>?> GetProcessesByUserIdAsync(string userId);

        Task AddProcessAsync(ProcessViewModel processViewModel);

        Task UpdateProcessAsync(ProcessViewModel processViewModel);

        Task DeleteProcessAsync(Process process);
    }
}
