using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Services;
using System.Security.Claims;

namespace EmployeeManagement.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var response = await _authService.RegisterAsync(request);
            
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return StatusCode(500, new AuthResponse
            {
                Success = false,
                Message = "An error occurred during registration"
            });
        }
    }

    /// <summary>
    /// Login user
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = await _authService.LoginAsync(request);
            
            if (!response.Success)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return StatusCode(500, new AuthResponse
            {
                Success = false,
                Message = "An error occurred during login"
            });
        }
    }

    /// <summary>
    /// Get current user information
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserInfo>> GetCurrentUser()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
            {
                return Unauthorized();
            }

            var user = await _authService.GetUserByIdAsync(id);
            
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new UserInfo
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving current user");
            return StatusCode(500, "An error occurred while retrieving user information");
        }
    }

    /// <summary>
    /// Validate token
    /// </summary>
    [HttpGet("validate")]
    [Authorize]
    public IActionResult ValidateToken()
    {
        return Ok(new { valid = true, message = "Token is valid" });
    }

    /// <summary>
    /// Get all users (Admin only)
    /// </summary>
    [HttpGet("users")]
    [Authorize]
    public async Task<ActionResult<List<UserInfo>>> GetAllUsers()
    {
        try
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int id))
            {
                return Unauthorized();
            }

            var currentUser = await _authService.GetUserByIdAsync(id);
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.ManageUsers))
            {
                return Forbid();
            }

            var users = await _authService.GetAllUsersAsync();
            var userInfos = users.Select(u =>
            {
                var roleInfo = RoleHelper.GetRoleInfo(u.Role);
                return new UserInfo
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    FullName = u.FullName,
                    Role = roleInfo.Name,
                    RoleLevel = roleInfo.Level,
                    Permissions = roleInfo.Permissions
                };
            }).ToList();

            return Ok(userInfos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode(500, "An error occurred while retrieving users");
        }
    }

    /// <summary>
    /// Change user role (Admin only)
    /// </summary>
    [HttpPost("change-role")]
    [Authorize]
    public async Task<ActionResult> ChangeUserRole([FromBody] ChangeRoleRequest request)
    {
        try
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int id))
            {
                return Unauthorized();
            }

            var currentUser = await _authService.GetUserByIdAsync(id);
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.AssignRoles))
            {
                return Forbid();
            }

            // Prevent changing own role
            if (request.UserId == currentUser.Id)
            {
                return BadRequest(new { message = "Cannot change your own role" });
            }

            var success = await _authService.ChangeUserRoleAsync(request.UserId, request.NewRole);
            if (!success)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { message = "Role changed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing user role");
            return StatusCode(500, "An error occurred while changing user role");
        }
    }

    /// <summary>
    /// Get available roles
    /// </summary>
    [HttpGet("roles")]
    [Authorize]
    public ActionResult<List<RoleInfo>> GetRoles()
    {
        var roles = RoleHelper.RoleDefinitions.Values.OrderBy(r => r.Level).ToList();
        return Ok(roles);
    }

    /// <summary>
    /// Delete user (Admin only)
    /// </summary>
    [HttpDelete("delete-user/{userId}")]
    [Authorize]
    public async Task<ActionResult> DeleteUser(int userId)
    {
        try
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int id))
            {
                return Unauthorized();
            }

            var currentUser = await _authService.GetUserByIdAsync(id);
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.ManageUsers))
            {
                return Forbid();
            }

            // Prevent deleting yourself
            if (userId == currentUser.Id)
            {
                return BadRequest(new { message = "Cannot delete your own account" });
            }

            var success = await _authService.DeleteUserAsync(userId);
            if (!success)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { message = "User deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user");
            return StatusCode(500, "An error occurred while deleting user");
        }
    }

    /// <summary>
    /// Create user (Admin only)
    /// </summary>
    [HttpPost("create-user")]
    [Authorize]
    public async Task<ActionResult<UserInfo>> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int id))
            {
                return Unauthorized();
            }

            var currentUser = await _authService.GetUserByIdAsync(id);
            if (currentUser == null || !_authService.HasPermission(currentUser, Permissions.ManageUsers))
            {
                return Forbid();
            }

            // Use the register method but with specified role
            var registerRequest = new RegisterRequest
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password,
                FullName = request.FullName
            };

            var result = await _authService.RegisterAsync(registerRequest);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            // Change role if specified and different from default
            if (request.Role.HasValue && request.Role.Value != UserRole.Viewer && result.User != null)
            {
                await _authService.ChangeUserRoleAsync(result.User.Id, request.Role.Value);
                
                // Get updated user info
                var user = await _authService.GetUserByIdAsync(result.User.Id);
                if (user != null)
                {
                    var roleInfo = RoleHelper.GetRoleInfo(user.Role);
                    result.User.Role = roleInfo.Name;
                    result.User.RoleLevel = roleInfo.Level;
                    result.User.Permissions = roleInfo.Permissions;
                }
            }

            return Ok(result.User);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return StatusCode(500, "An error occurred while creating user");
        }
    }
}

/// <summary>
/// Request to create a new user (Admin only)
/// </summary>
public class CreateUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public UserRole? Role { get; set; }
}
