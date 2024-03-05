using CaratCount.Data;
using CaratCount.Entities;
using CaratCount.Interface;
using CaratCount.Migrations;
using Microsoft.EntityFrameworkCore;

namespace CaratCount.Managers
{
    public class EmployeeManager: IEmployeeManager
    {
        private readonly ApplicationDbContext _context;

        public EmployeeManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee?> GetEmployeeByIdAsync(string id, string userId)
        {
            Guid employeeId;

            if (!Guid.TryParse(id, out employeeId) || string.IsNullOrEmpty(userId)) return null;

            Employee? employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == employeeId && e.UserId == userId);

            return employee;
        }

        public async Task<List<Employee>?> GetEmployeesByUserIdAsync(string userId)
        {
            List<Employee> employees = await _context.Employees
                .Where(e => e.UserId == userId)
                .ToListAsync();
              

            return employees;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            try
            {
                _context.Employees.Add(employee);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {

            Employee? existingEmployee = await _context.Employees.FindAsync(employee.Id);

            if (existingEmployee == null)
            {
                throw new ArgumentException("Employee not found.");
            }

            try
            {
                existingEmployee.Name = employee.Name;
                existingEmployee.Email = employee.Email;
                existingEmployee.PhoneNumber = employee.PhoneNumber;

                _context.Employees.Update(existingEmployee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task DeleteEmployeeAsync(Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    throw new ArgumentNullException(nameof(employee));
                }

                _context.Employees.Remove(employee);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
