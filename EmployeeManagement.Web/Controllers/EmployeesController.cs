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
    private readonly IExportImportService _exportImportService;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(
        IEmployeeService employeeService, 
        IAuthService authService, 
        IExportImportService exportImportService,
        ILogger<EmployeesController> logger)
    {
        _employeeService = employeeService;
        _authService = authService;
        _exportImportService = exportImportService;
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

    /// <summary>
    /// Search, filter, and sort employees
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Employee>>> SearchEmployees(
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? department = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = false)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.ViewEmployees))
            {
                return Forbid();
            }

            var employees = await _employeeService.SearchEmployeesAsync(searchTerm, department, sortBy, sortDescending);
            return Ok(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching employees");
            return StatusCode(500, "An error occurred while searching employees");
        }
    }

    /// <summary>
    /// Get list of all departments
    /// </summary>
    [HttpGet("departments")]
    public async Task<ActionResult<IEnumerable<string>>> GetDepartments()
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.ViewEmployees))
            {
                return Forbid();
            }

            var departments = await _employeeService.GetDepartmentsAsync();
            return Ok(departments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving departments");
            return StatusCode(500, "An error occurred while retrieving departments");
        }
    }

    /// <summary>
    /// Export employees to CSV
    /// </summary>
    [HttpGet("export/csv")]
    public async Task<IActionResult> ExportCsv([FromQuery] string? searchTerm = null, [FromQuery] string? department = null)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.ViewEmployees))
            {
                return Forbid();
            }

            var employees = await _employeeService.SearchEmployeesAsync(searchTerm, department);
            var csvData = await _exportImportService.ExportToCsvAsync(employees);
            
            return File(csvData, "text/csv", $"employees_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting to CSV");
            return StatusCode(500, "An error occurred while exporting to CSV");
        }
    }

    /// <summary>
    /// Export employees to Excel
    /// </summary>
    [HttpGet("export/excel")]
    public async Task<IActionResult> ExportExcel([FromQuery] string? searchTerm = null, [FromQuery] string? department = null)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.ViewEmployees))
            {
                return Forbid();
            }

            var employees = await _employeeService.SearchEmployeesAsync(searchTerm, department);
            var excelData = await _exportImportService.ExportToExcelAsync(employees);
            
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"employees_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting to Excel");
            return StatusCode(500, "An error occurred while exporting to Excel");
        }
    }

    /// <summary>
    /// Export employees to PDF
    /// </summary>
    [HttpGet("export/pdf")]
    public async Task<IActionResult> ExportPdf([FromQuery] string? searchTerm = null, [FromQuery] string? department = null)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.ViewEmployees))
            {
                return Forbid();
            }

            var employees = await _employeeService.SearchEmployeesAsync(searchTerm, department);
            var pdfData = await _exportImportService.ExportToPdfAsync(employees);
            
            return File(pdfData, "application/pdf", $"employees_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting to PDF");
            return StatusCode(500, "An error occurred while exporting to PDF");
        }
    }

    /// <summary>
    /// Import employees from CSV file
    /// </summary>
    [HttpPost("import/csv")]
    public async Task<ActionResult<ImportResult>> ImportCsv(IFormFile file)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.AddEmployees))
            {
                return Forbid();
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("File must be a CSV file");
            }

            using var stream = file.OpenReadStream();
            var importResult = await _exportImportService.ImportFromCsvAsync(stream);

            // Add successfully imported employees to the system
            foreach (var employee in importResult.ImportedEmployees)
            {
                try
                {
                    await _employeeService.AddEmployeeAsync(employee);
                }
                catch (InvalidOperationException ex)
                {
                    importResult.Errors.Add($"Employee ID {employee.Id}: {ex.Message}");
                    importResult.SuccessfulImports--;
                    importResult.FailedImports++;
                }
            }

            return Ok(importResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing from CSV");
            return StatusCode(500, "An error occurred while importing from CSV");
        }
    }
}
