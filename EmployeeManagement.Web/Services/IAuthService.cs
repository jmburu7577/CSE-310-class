using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByIdAsync(int id);
    Task<List<User>> GetAllUsersAsync();
    Task<bool> ChangeUserRoleAsync(int userId, UserRole newRole);
    Task<bool> DeleteUserAsync(int userId);
    string GenerateJwtToken(User user);
    bool HasPermission(User user, string permission);
}
