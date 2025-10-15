using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Services;

namespace EmployeeManagement.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IAuthService _authService;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(IEmployeeService employeeService, IAuthService authService, ILogger<EmployeesController> logger)
    {
        _employeeService = employeeService;
        _authService = authService;
        _logger = logger;
    }

    private async Task<User?> GetCurrentUserAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
        {
            return null;
        }
        return await _authService.GetUserByIdAsync(id);
    }

    /// <summary>
    /// Get all employees
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.ViewEmployees))
            {
                return Forbid();
            }

            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees");
            return StatusCode(500, "An error occurred while retrieving employees");
        }
    }

    /// <summary>
    /// Get employee by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }
            return Ok(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the employee");
        }
    }

    /// <summary>
    /// Create a new employee
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Employee>> CreateEmployee([FromBody] Employee employee)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.AddEmployees))
            {
                return Forbid();
            }

            var created = await _employeeService.AddEmployeeAsync(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee");
            return StatusCode(500, "An error occurred while creating the employee");
        }
    }

    /// <summary>
    /// Update an existing employee
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<Employee>> UpdateEmployee(int id, [FromBody] Employee employee)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.EditEmployees))
            {
                return Forbid();
            }

            var updated = await _employeeService.UpdateEmployeeAsync(id, employee);
            if (updated == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }
            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee {Id}", id);
            return StatusCode(500, "An error occurred while updating the employee");
        }
    }

    /// <summary>
    /// Delete an employee
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEmployee(int id)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.DeleteEmployees))
            {
                return Forbid();
            }

            var deleted = await _employeeService.DeleteEmployeeAsync(id);
            if (!deleted)
            {
                return NotFound($"Employee with ID {id} not found");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee {Id}", id);
            return StatusCode(500, "An error occurred while deleting the employee");
        }
    }

    /// <summary>
    /// Get employee statistics
    /// </summary>
    [HttpGet("stats")]
    public async Task<ActionResult<object>> GetStats()
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.ViewStatistics))
            {
                return Forbid();
            }

            var employees = await _employeeService.GetAllEmployeesAsync();
            var stats = new
            {
                TotalEmployees = employees.Count(),
                AverageSalary = employees.Any() ? employees.Average(e => e.Salary) : 0,
                AverageAge = employees.Any() ? employees.Average(e => e.Age) : 0,
                DepartmentCount = employees.Select(e => e.Department).Distinct().Count(),
                Departments = employees.GroupBy(e => e.Department)
                    .Select(g => new { Department = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
            };
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving statistics");
            return StatusCode(500, "An error occurred while retrieving statistics");
        }
    }
}
