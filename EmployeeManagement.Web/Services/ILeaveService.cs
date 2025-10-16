using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

/// <summary>
/// Interface for leave management operations
/// </summary>
public interface ILeaveService
{
    // Leave Requests
    Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync();
    Task<LeaveRequest?> GetLeaveRequestByIdAsync(int id);
    Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<LeaveRequest>> GetPendingLeaveRequestsAsync();
    Task<LeaveRequest> CreateLeaveRequestAsync(int employeeId, CreateLeaveRequest request);
    Task<LeaveRequest?> ApproveLeaveRequestAsync(int leaveId, int approverId, ApproveLeaveRequest request);
    Task<LeaveRequest?> RejectLeaveRequestAsync(int leaveId, int approverId, ApproveLeaveRequest request);
    Task<bool> CancelLeaveRequestAsync(int leaveId, int employeeId);
    Task<bool> DeleteLeaveRequestAsync(int id);
    
    // Leave Balances
    Task<LeaveBalance?> GetLeaveBalanceAsync(int employeeId, int year);
    Task<LeaveBalance> InitializeLeaveBalanceAsync(int employeeId, int year);
    Task UpdateLeaveBalanceAsync(int employeeId, LeaveType leaveType, decimal days, bool isUsed);
    
    // Statistics
    Task<LeaveStatistics> GetLeaveStatisticsAsync();
}
