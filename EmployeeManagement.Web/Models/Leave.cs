namespace EmployeeManagement.Web.Models;

/// <summary>
/// Represents a leave request from an employee
/// </summary>
public class LeaveRequest
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public LeaveType Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Days { get; set; }
    public string Reason { get; set; } = string.Empty;
    public LeaveStatus Status { get; set; }
    public int? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApproverNotes { get; set; }
    public DateTime RequestedDate { get; set; }
}

/// <summary>
/// Leave type enumeration
/// </summary>
public enum LeaveType
{
    Vacation = 1,
    Sick = 2,
    Unpaid = 3,
    Personal = 4,
    Maternity = 5,
    Paternity = 6,
    Bereavement = 7,
    Study = 8
}

/// <summary>
/// Leave request status
/// </summary>
public enum LeaveStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Cancelled = 4
}

/// <summary>
/// Employee leave balance tracking
/// </summary>
public class LeaveBalance
{
    public int EmployeeId { get; set; }
    public int Year { get; set; }
    public decimal VacationDays { get; set; }
    public decimal SickDays { get; set; }
    public decimal PersonalDays { get; set; }
    public decimal VacationUsed { get; set; }
    public decimal SickUsed { get; set; }
    public decimal PersonalUsed { get; set; }
    
    public decimal VacationRemaining => VacationDays - VacationUsed;
    public decimal SickRemaining => SickDays - SickUsed;
    public decimal PersonalRemaining => PersonalDays - PersonalUsed;
}

/// <summary>
/// Request to create a leave
/// </summary>
public class CreateLeaveRequest
{
    public LeaveType Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
}

/// <summary>
/// Request to approve or reject a leave
/// </summary>
public class ApproveLeaveRequest
{
    public bool Approve { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Leave request with employee details
/// </summary>
public class LeaveRequestWithEmployee
{
    public LeaveRequest LeaveRequest { get; set; } = null!;
    public Employee Employee { get; set; } = null!;
    public User? Approver { get; set; }
}

/// <summary>
/// Leave statistics
/// </summary>
public class LeaveStatistics
{
    public int TotalRequests { get; set; }
    public int PendingRequests { get; set; }
    public int ApprovedRequests { get; set; }
    public int RejectedRequests { get; set; }
    public Dictionary<LeaveType, int> RequestsByType { get; set; } = new();
    public List<LeaveRequestWithEmployee> RecentRequests { get; set; } = new();
}
