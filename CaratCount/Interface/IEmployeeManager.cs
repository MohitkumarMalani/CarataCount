using CaratCount.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaratCount.Interface
{
    public interface IEmployeeManager
    {
        Task<Employee?> GetEmployeeByIdAsync(string id, string userId);

        Task<List<Employee>?> GetEmployeesByUserIdAsync(string userId);

        Task AddEmployeeAsync(Employee employee);

        Task UpdateEmployeeAsync(Employee employee);

        Task DeleteEmployeeAsync(Employee employee);
    }
}
