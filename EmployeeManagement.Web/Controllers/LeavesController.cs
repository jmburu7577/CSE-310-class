using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Services;

namespace EmployeeManagement.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LeavesController : ControllerBase
{
    private readonly ILeaveService _leaveService;
    private readonly IEmployeeService _employeeService;
    private readonly IAuthService _authService;
    private readonly ILogger<LeavesController> _logger;

    public LeavesController(
        ILeaveService leaveService,
        IEmployeeService employeeService,
        IAuthService authService,
        ILogger<LeavesController> logger)
    {
        _leaveService = leaveService;
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
    /// Get all leave requests (Senior+ only)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeaveRequest>>> GetAllLeaveRequests()
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || currentUser.Role < UserRole.Senior)
            {
                return Forbid();
            }

            var leaveRequests = await _leaveService.GetAllLeaveRequestsAsync();
            return Ok(leaveRequests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all leave requests");
            return StatusCode(500, "An error occurred while retrieving leave requests");
        }
    }

    /// <summary>
    /// Get leave request by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<LeaveRequest>> GetLeaveRequest(int id)
    {
        try
        {
            var leaveRequest = await _leaveService.GetLeaveRequestByIdAsync(id);
            if (leaveRequest == null)
            {
                return NotFound($"Leave request with ID {id} not found");
            }

            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return Forbid();
            }

            // Users can view their own requests or if they're senior+
            // TODO: Link user to employee to check ownership
            if (currentUser.Role < UserRole.Senior)
            {
                return Forbid();
            }

            return Ok(leaveRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving leave request {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the leave request");
        }
    }

    /// <summary>
    /// Get all leave requests for a specific employee
    /// </summary>
    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<LeaveRequest>>> GetEmployeeLeaveRequests(int employeeId)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return Forbid();
            }

            // TODO: Check if user owns this employee record or is senior+
            if (currentUser.Role < UserRole.Senior)
            {
                return Forbid();
            }

            var leaveRequests = await _leaveService.GetLeaveRequestsByEmployeeIdAsync(employeeId);
            return Ok(leaveRequests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving leave requests for employee {EmployeeId}", employeeId);
            return StatusCode(500, "An error occurred while retrieving employee leave requests");
        }
    }

    /// <summary>
    /// Get all pending leave requests (Senior+ only)
    /// </summary>
    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<LeaveRequest>>> GetPendingLeaveRequests()
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || currentUser.Role < UserRole.Senior)
            {
                return Forbid();
            }

            var pendingRequests = await _leaveService.GetPendingLeaveRequestsAsync();
            return Ok(pendingRequests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving pending leave requests");
            return StatusCode(500, "An error occurred while retrieving pending leave requests");
        }
    }

    /// <summary>
    /// Create a new leave request for an employee
    /// </summary>
    [HttpPost("employee/{employeeId}")]
    public async Task<ActionResult<LeaveRequest>> CreateLeaveRequest(
        int employeeId,
        [FromBody] CreateLeaveRequest request)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return Forbid();
            }

            // TODO: Check if user owns this employee record or has AddEmployees permission
            if (!_authService.HasPermission(currentUser, Permissions.AddEmployees))
            {
                return Forbid();
            }

            var leaveRequest = await _leaveService.CreateLeaveRequestAsync(employeeId, request);
            return CreatedAtAction(nameof(GetLeaveRequest), new { id = leaveRequest.Id }, leaveRequest);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating leave request for employee {EmployeeId}", employeeId);
            return StatusCode(500, "An error occurred while creating the leave request");
        }
    }

    /// <summary>
    /// Approve a leave request (Senior+ only)
    /// </summary>
    [HttpPost("{id}/approve")]
    public async Task<ActionResult<LeaveRequest>> ApproveLeaveRequest(
        int id,
        [FromBody] ApproveLeaveRequest request)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || currentUser.Role < UserRole.Senior)
            {
                return Forbid();
            }

            var approvedRequest = await _leaveService.ApproveLeaveRequestAsync(id, currentUser.Id, request);
            if (approvedRequest == null)
            {
                return NotFound($"Leave request with ID {id} not found");
            }

            return Ok(approvedRequest);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving leave request {Id}", id);
            return StatusCode(500, "An error occurred while approving the leave request");
        }
    }

    /// <summary>
    /// Reject a leave request (Senior+ only)
    /// </summary>
    [HttpPost("{id}/reject")]
    public async Task<ActionResult<LeaveRequest>> RejectLeaveRequest(
        int id,
        [FromBody] ApproveLeaveRequest request)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || currentUser.Role < UserRole.Senior)
            {
                return Forbid();
            }

            var rejectedRequest = await _leaveService.RejectLeaveRequestAsync(id, currentUser.Id, request);
            if (rejectedRequest == null)
            {
                return NotFound($"Leave request with ID {id} not found");
            }

            return Ok(rejectedRequest);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting leave request {Id}", id);
            return StatusCode(500, "An error occurred while rejecting the leave request");
        }
    }

    /// <summary>
    /// Cancel a leave request (Employee can cancel their own pending request)
    /// </summary>
    [HttpPost("{id}/cancel")]
    public async Task<ActionResult> CancelLeaveRequest(int id, [FromQuery] int employeeId)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return Forbid();
            }

            // TODO: Verify user owns this employee record

            var cancelled = await _leaveService.CancelLeaveRequestAsync(id, employeeId);
            if (!cancelled)
            {
                return NotFound($"Leave request with ID {id} not found");
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling leave request {Id}", id);
            return StatusCode(500, "An error occurred while cancelling the leave request");
        }
    }

    /// <summary>
    /// Delete a leave request (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLeaveRequest(int id)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || currentUser.Role < UserRole.Admin)
            {
                return Forbid();
            }

            var deleted = await _leaveService.DeleteLeaveRequestAsync(id);
            if (!deleted)
            {
                return NotFound($"Leave request with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting leave request {Id}", id);
            return StatusCode(500, "An error occurred while deleting the leave request");
        }
    }

    /// <summary>
    /// Get leave balance for an employee
    /// </summary>
    [HttpGet("balance/{employeeId}")]
    public async Task<ActionResult<LeaveBalance>> GetLeaveBalance(
        int employeeId,
        [FromQuery] int? year = null)
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return Forbid();
            }

            // TODO: Check if user owns this employee record or is senior+
            if (currentUser.Role < UserRole.Senior)
            {
                return Forbid();
            }

            var targetYear = year ?? DateTime.Now.Year;
            var balance = await _leaveService.GetLeaveBalanceAsync(employeeId, targetYear);

            if (balance == null)
            {
                // Initialize balance for the year if it doesn't exist
                balance = await _leaveService.InitializeLeaveBalanceAsync(employeeId, targetYear);
            }

            return Ok(balance);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving leave balance for employee {EmployeeId}", employeeId);
            return StatusCode(500, "An error occurred while retrieving the leave balance");
        }
    }

    /// <summary>
    /// Get leave statistics (Senior+ only)
    /// </summary>
    [HttpGet("stats")]
    public async Task<ActionResult<LeaveStatistics>> GetLeaveStatistics()
    {
        try
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || currentUser.Role < UserRole.Senior)
            {
                return Forbid();
            }

            var stats = await _leaveService.GetLeaveStatisticsAsync();
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving leave statistics");
            return StatusCode(500, "An error occurred while retrieving leave statistics");
        }
    }
}
