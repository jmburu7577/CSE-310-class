using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

public class AuthService : IAuthService
{
    private readonly string _usersFilePath = "users.json";
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private List<User> _users;

    public AuthService(IConfiguration configuration, ILogger<AuthService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _users = LoadUsers();
        SeedDefaultAdminAsync().Wait();
    }

    private async Task SeedDefaultAdminAsync()
    {
        var defaultAdmins = new[]
        {
            new { Username = "admin", Email = "admin@employeemanagement.com", FullName = "System Administrator", Password = "admin123" },
            new { Username = "jemo", Email = "jemo@employeemanagement.com", FullName = "Jemo Administrator", Password = "jemo123" },
            new { Username = "testuser", Email = "testuser@employeemanagement.com", FullName = "Test User Administrator", Password = "test123" }
        };

        bool anyCreated = false;

        foreach (var adminData in defaultAdmins)
        {
            // Check if admin already exists
            if (_users.Any(u => u.Username.Equals(adminData.Username, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            // Create admin account
            var admin = new User
            {
                Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1,
                Username = adminData.Username,
                Email = adminData.Email,
                FullName = adminData.FullName,
                PasswordHash = HashPassword(adminData.Password),
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            };

            _users.Add(admin);
            anyCreated = true;
            
            _logger.LogInformation($"Default admin account created - Username: {adminData.Username}, Password: {adminData.Password}");
        }

        if (anyCreated)
        {
            await SaveUsers();
        }
    }

    private List<User> LoadUsers()
    {
        try
        {
            if (File.Exists(_usersFilePath))
            {
                var json = File.ReadAllText(_usersFilePath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading users from file");
        }
        return new List<User>();
    }

    private async Task SaveUsers()
    {
        try
        {
            var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_usersFilePath, json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving users to file");
            throw;
        }
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(request.Username) || 
            string.IsNullOrWhiteSpace(request.Email) || 
            string.IsNullOrWhiteSpace(request.Password))
        {
            return new AuthResponse
            {
                Success = false,
                Message = "All fields are required"
            };
        }

        // Check if username already exists
        if (_users.Any(u => u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase)))
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Username already exists"
            };
        }

        // Check if email already exists
        if (_users.Any(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Email already exists"
            };
        }

        // Validate password strength
        if (request.Password.Length < 6)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Password must be at least 6 characters long"
            };
        }

        // Create new user - First user is Admin, others are Viewer by default
        var isFirstUser = !_users.Any();
        var user = new User
        {
            Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1,
            Username = request.Username,
            Email = request.Email,
            FullName = request.FullName,
            PasswordHash = HashPassword(request.Password),
            Role = isFirstUser ? UserRole.Admin : UserRole.Viewer,
            CreatedAt = DateTime.UtcNow
        };

        _users.Add(user);
        await SaveUsers();

        var token = GenerateJwtToken(user);
        var roleInfo = RoleHelper.GetRoleInfo(user.Role);

        return new AuthResponse
        {
            Success = true,
            Message = "Registration successful",
            Token = token,
            User = new UserInfo
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Role = roleInfo.Name,
                RoleLevel = roleInfo.Level,
                Permissions = roleInfo.Permissions
            }
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Username and password are required"
            };
        }

        // Find user
        var user = _users.FirstOrDefault(u => 
            u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase));

        if (user == null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Invalid username or password"
            };
        }

        // Verify password
        if (!VerifyPassword(request.Password, user.PasswordHash))
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Invalid username or password"
            };
        }

        // Update last login
        user.LastLogin = DateTime.UtcNow;
        await SaveUsers();

        var token = GenerateJwtToken(user);
        var roleInfo = RoleHelper.GetRoleInfo(user.Role);

        return new AuthResponse
        {
            Success = true,
            Message = "Login successful",
            Token = token,
            User = new UserInfo
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Role = roleInfo.Name,
                RoleLevel = roleInfo.Level,
                Permissions = roleInfo.Permissions
            }
        };
    }

    public Task<User?> GetUserByUsernameAsync(string username)
    {
        var user = _users.FirstOrDefault(u => 
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user);
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<List<User>> GetAllUsersAsync()
    {
        return Task.FromResult(_users.ToList());
    }

    public async Task<bool> ChangeUserRoleAsync(int userId, UserRole newRole)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }

        user.Role = newRole;
        await SaveUsers();
        return true;
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }

        _users.Remove(user);
        await SaveUsers();
        _logger.LogInformation($"User deleted - ID: {userId}, Username: {user.Username}");
        return true;
    }

    public bool HasPermission(User user, string permission)
    {
        return RoleHelper.HasPermission(user.Role, permission);
    }

    public string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? "YourSuperSecretKeyForJWTTokenGeneration123456789";
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "EmployeeManagementApp";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "EmployeeManagementApp";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("FullName", user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("RoleLevel", ((int)user.Role).ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string hash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput.Equals(hash);
    }
}
