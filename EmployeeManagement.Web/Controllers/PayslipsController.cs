using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Services;

namespace EmployeeManagement.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PayslipsController : ControllerBase
{
    private readonly IPayslipService _payslipService;
    private readonly IEmployeeService _employeeService;
    private readonly IAuthService _authService;
    private readonly ILogger<PayslipsController> _logger;

    public PayslipsController(
        IPayslipService payslipService,
        IEmployeeService employeeService,
        IAuthService authService,
        ILogger<PayslipsController> logger)
    {
        _payslipService = payslipService;
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
    /// Get all payslips (Admin only)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Payslip>>> GetAllPayslips()
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.ViewEmployees))
            {
                return Forbid();
            }

            var payslips = await _payslipService.GetAllPayslipsAsync();
            return Ok(payslips);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all payslips");
            return StatusCode(500, "An error occurred while retrieving payslips");
        }
    }

    /// <summary>
    /// Get payslip by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Payslip>> GetPayslip(int id)
    {
        try
        {
            var payslip = await _payslipService.GetPayslipByIdAsync(id);
            if (payslip == null)
            {
                return NotFound($"Payslip with ID {id} not found");
            }

            // Check if user can access this payslip
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return Forbid();
            }

            // Employees can only view their own payslips
            // Admins/Managers can view all payslips
            if (currentUser.Role < UserRole.Manager)
            {
                // TODO: Add employee-user mapping to check if this is their payslip
                // For now, allow viewing if they have ViewEmployees permission
                if (!_authService.HasPermission(currentUser, Permissions.ViewEmployees))
                {
                    return Forbid();
                }
            }

            return Ok(payslip);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payslip {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the payslip");
        }
    }

    /// <summary>
    /// Get all payslips for a specific employee
    /// </summary>
    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<Payslip>>> GetEmployeePayslips(int employeeId)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.ViewEmployees))
            {
                return Forbid();
            }

            var payslips = await _payslipService.GetPayslipsByEmployeeIdAsync(employeeId);
            return Ok(payslips);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payslips for employee {EmployeeId}", employeeId);
            return StatusCode(500, "An error occurred while retrieving employee payslips");
        }
    }

    /// <summary>
    /// Generate payslip for a single employee
    /// </summary>
    [HttpPost("generate/{employeeId}")]
    public async Task<ActionResult<PayslipGenerationResult>> GeneratePayslip(
        int employeeId,
        [FromBody] GeneratePayslipRequest request)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || currentUser.Role < UserRole.Manager)
            {
                return Forbid();
            }

            var result = await _payslipService.GeneratePayslipAsync(employeeId, request);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating payslip for employee {EmployeeId}", employeeId);
            return StatusCode(500, "An error occurred while generating the payslip");
        }
    }

    /// <summary>
    /// Generate payslips for all employees (bulk generation)
    /// </summary>
    [HttpPost("generate-bulk")]
    public async Task<ActionResult<PayslipGenerationResult>> GenerateBulkPayslips(
        [FromBody] GeneratePayslipRequest request)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || currentUser.Role < UserRole.Senior)
            {
                return Forbid();
            }

            var result = await _payslipService.GenerateBulkPayslipsAsync(request);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in bulk payslip generation");
            return StatusCode(500, "An error occurred during bulk payslip generation");
        }
    }

    /// <summary>
    /// Download payslip as PDF
    /// </summary>
    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadPayslip(int id)
    {
        try
        {
            var payslip = await _payslipService.GetPayslipByIdAsync(id);
            if (payslip == null)
            {
                return NotFound($"Payslip with ID {id} not found");
            }

            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return Forbid();
            }

            // Mark as viewed when downloaded
            await _payslipService.MarkPayslipAsViewedAsync(id);

            var pdfData = await _payslipService.GeneratePayslipPdfAsync(id);
            
            return File(pdfData, "application/pdf", $"payslip_{id}_{DateTime.Now:yyyyMMdd}.pdf");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading payslip {Id}", id);
            return StatusCode(500, "An error occurred while downloading the payslip");
        }
    }

    /// <summary>
    /// Delete payslip (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePayslip(int id)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || currentUser.Role < UserRole.Admin)
            {
                return Forbid();
            }

            var deleted = await _payslipService.DeletePayslipAsync(id);
            if (!deleted)
            {
                return NotFound($"Payslip with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payslip {Id}", id);
            return StatusCode(500, "An error occurred while deleting the payslip");
        }
    }

    /// <summary>
    /// Get payslip statistics
    /// </summary>
    [HttpGet("stats")]
    public async Task<ActionResult<object>> GetPayslipStats()
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || currentUser.Role < UserRole.Manager)
            {
                return Forbid();
            }

            var payslips = await _payslipService.GetAllPayslipsAsync();
            var stats = new
            {
                TotalPayslips = payslips.Count(),
                TotalAmount = payslips.Sum(p => p.NetSalary),
                AverageNetSalary = payslips.Any() ? payslips.Average(p => p.NetSalary) : 0,
                Generated = payslips.Count(p => p.Status == PayslipStatus.Generated),
                Sent = payslips.Count(p => p.Status == PayslipStatus.Sent),
                Viewed = payslips.Count(p => p.Status == PayslipStatus.Viewed),
                RecentPayslips = payslips.Take(10).Select(p => new
                {
                    p.Id,
                    p.EmployeeId,
                    p.NetSalary,
                    p.GeneratedDate,
                    p.Status
                })
            };

            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payslip statistics");
            return StatusCode(500, "An error occurred while retrieving statistics");
        }
    }
}
