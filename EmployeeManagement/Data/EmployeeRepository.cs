using System.Text.Json;
using EmployeeManagement.Models;

namespace EmployeeManagement.Data;

public class EmployeeRepository
{
    private readonly List<Employee> _employees = new();

    public IReadOnlyList<Employee> Employees => _employees;

    public void AddEmployee(Employee employee)
    {
        if (_employees.Any(e => e.Id == employee.Id))
        {
            throw new InvalidOperationException($"Employee with ID {employee.Id} already exists.");
        }
        _employees.Add(employee);
    }

    public Employee? FindById(int id)
        => _employees.FirstOrDefault(e => e.Id == id);

    public void SaveToFile(string path)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var json = JsonSerializer.Serialize(_employees, options);
        File.WriteAllText(path, json);
    }

    public void LoadFromFile(string path)
    {
        if (!File.Exists(path))
        {
            _employees.Clear();
            return;
        }
        var json = File.ReadAllText(path);
        var data = JsonSerializer.Deserialize<List<Employee>>(json) ?? new List<Employee>();
        _employees.Clear();
        _employees.AddRange(data);
    }
}
