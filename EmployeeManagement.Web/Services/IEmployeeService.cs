using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

/// <summary>
/// Interface for employee management operations
/// </summary>
public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<Employee?> GetEmployeeByIdAsync(int id);
    Task<Employee> AddEmployeeAsync(Employee employee);
    Task<Employee?> UpdateEmployeeAsync(int id, Employee employee);
    Task<bool> DeleteEmployeeAsync(int id);
    Task<int> GetEmployeeCountAsync();
    
    // Search, Filter & Sort
    Task<IEnumerable<Employee>> SearchEmployeesAsync(string? searchTerm = null, 
        string? department = null, 
        string? sortBy = null, 
        bool sortDescending = false);
    Task<IEnumerable<string>> GetDepartmentsAsync();
}
