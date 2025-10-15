namespace EmployeeManagement.Web.Models;

/// <summary>
/// User roles based on management level
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Entry level - Can only view employees
    /// </summary>
    Viewer = 1,
    
    /// <summary>
    /// Junior management - Can view and add employees
    /// </summary>
    Employee = 2,
    
    /// <summary>
    /// Mid-level management - Can view, add, and edit employees
    /// </summary>
    Manager = 3,
    
    /// <summary>
    /// Senior management - Can view, add, edit, and delete employees
    /// </summary>
    Senior = 4,
    
    /// <summary>
    /// Top level - Full access including user management
    /// </summary>
    Admin = 5
}

/// <summary>
/// Permissions available in the system
/// </summary>
public static class Permissions
{
    public const string ViewEmployees = "Employees.View";
    public const string AddEmployees = "Employees.Add";
    public const string EditEmployees = "Employees.Edit";
    public const string DeleteEmployees = "Employees.Delete";
    public const string ViewStatistics = "Statistics.View";
    public const string ManageUsers = "Users.Manage";
    public const string AssignRoles = "Roles.Assign";
}

/// <summary>
/// Role information with permissions
/// </summary>
public class RoleInfo
{
    public UserRole Role { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Level { get; set; }
    public List<string> Permissions { get; set; } = new();
}

/// <summary>
/// Helper class for role management
/// </summary>
public static class RoleHelper
{
    public static readonly Dictionary<UserRole, RoleInfo> RoleDefinitions = new()
    {
        {
            UserRole.Viewer, new RoleInfo
            {
                Role = UserRole.Viewer,
                Name = "Viewer",
                Description = "Can only view employee information",
                Level = 1,
                Permissions = new List<string>
                {
                    Permissions.ViewEmployees,
                    Permissions.ViewStatistics
                }
            }
        },
        {
            UserRole.Employee, new RoleInfo
            {
                Role = UserRole.Employee,
                Name = "Employee",
                Description = "Can view and add employees",
                Level = 2,
                Permissions = new List<string>
                {
                    Permissions.ViewEmployees,
                    Permissions.AddEmployees,
                    Permissions.ViewStatistics
                }
            }
        },
        {
            UserRole.Manager, new RoleInfo
            {
                Role = UserRole.Manager,
                Name = "Manager",
                Description = "Can view, add, and edit employees",
                Level = 3,
                Permissions = new List<string>
                {
                    Permissions.ViewEmployees,
                    Permissions.AddEmployees,
                    Permissions.EditEmployees,
                    Permissions.ViewStatistics
                }
            }
        },
        {
            UserRole.Senior, new RoleInfo
            {
                Role = UserRole.Senior,
                Name = "Senior Manager",
                Description = "Can view, add, edit, and delete employees",
                Level = 4,
                Permissions = new List<string>
                {
                    Permissions.ViewEmployees,
                    Permissions.AddEmployees,
                    Permissions.EditEmployees,
                    Permissions.DeleteEmployees,
                    Permissions.ViewStatistics
                }
            }
        },
        {
            UserRole.Admin, new RoleInfo
            {
                Role = UserRole.Admin,
                Name = "Administrator",
                Description = "Full system access including user management",
                Level = 5,
                Permissions = new List<string>
                {
                    Permissions.ViewEmployees,
                    Permissions.AddEmployees,
                    Permissions.EditEmployees,
                    Permissions.DeleteEmployees,
                    Permissions.ViewStatistics,
                    Permissions.ManageUsers,
                    Permissions.AssignRoles
                }
            }
        }
    };

    public static RoleInfo GetRoleInfo(UserRole role)
    {
        return RoleDefinitions.TryGetValue(role, out var info) ? info : RoleDefinitions[UserRole.Viewer];
    }

    public static bool HasPermission(UserRole role, string permission)
    {
        var roleInfo = GetRoleInfo(role);
        return roleInfo.Permissions.Contains(permission);
    }

    public static List<string> GetPermissions(UserRole role)
    {
        return GetRoleInfo(role).Permissions;
    }
}
