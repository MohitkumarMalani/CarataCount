using CaratCount.Entities;

namespace CaratCount.Interface
{
    public interface IProcessManager
    {
        Task<Process?> GetProcessByIdAsync(string id, string userId);

        Task<List<Process>?> GetProcessesByUserIdAsync(string userId);

        Task AddProcessAsync(Process process);

        Task UpdateProcessAsync(Process process);

        Task DeleteProcessAsync(Process process);
    }
}
