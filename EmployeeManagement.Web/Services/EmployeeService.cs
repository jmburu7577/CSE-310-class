using System.Text.Json;
using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

/// <summary>
/// Service for managing employee data with file persistence
/// </summary>
public class EmployeeService : IEmployeeService
{
    private readonly List<Employee> _employees = new();
    private readonly string _dataFile;
    private readonly SemaphoreSlim _fileLock = new(1, 1);

    public EmployeeService()
    {
        _dataFile = Path.Combine(AppContext.BaseDirectory, "employees.json");
        LoadFromFileAsync().Wait();
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        await Task.CompletedTask;
        return _employees.ToList();
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        await Task.CompletedTask;
        return _employees.FirstOrDefault(e => e.Id == id);
    }

    public async Task<Employee> AddEmployeeAsync(Employee employee)
    {
        if (_employees.Any(e => e.Id == employee.Id))
        {
            throw new InvalidOperationException($"Employee with ID {employee.Id} already exists.");
        }

        _employees.Add(employee);
        await SaveToFileAsync();
        return employee;
    }

    public async Task<Employee?> UpdateEmployeeAsync(int id, Employee employee)
    {
        var existing = _employees.FirstOrDefault(e => e.Id == id);
        if (existing == null)
        {
            return null;
        }

        existing.FirstName = employee.FirstName;
        existing.LastName = employee.LastName;
        existing.Age = employee.Age;
        existing.Department = employee.Department;
        existing.Salary = employee.Salary;
        existing.Address = employee.Address;

        await SaveToFileAsync();
        return existing;
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        if (employee == null)
        {
            return false;
        }

        _employees.Remove(employee);
        await SaveToFileAsync();
        return true;
    }

    public async Task<int> GetEmployeeCountAsync()
    {
        await Task.CompletedTask;
        return _employees.Count;
    }

    private async Task SaveToFileAsync()
    {
        await _fileLock.WaitAsync();
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(_employees, options);
            await File.WriteAllTextAsync(_dataFile, json);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private async Task LoadFromFileAsync()
    {
        if (!File.Exists(_dataFile))
        {
            return;
        }

        await _fileLock.WaitAsync();
        try
        {
            var json = await File.ReadAllTextAsync(_dataFile);
            var data = JsonSerializer.Deserialize<List<Employee>>(json) ?? new List<Employee>();
            _employees.Clear();
            _employees.AddRange(data);
        }
        finally
        {
            _fileLock.Release();
        }
    }
}
