namespace EmployeeManagement.Web.Models;

/// <summary>
/// Represents a daily attendance record
/// </summary>
public class AttendanceRecord
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public DateTime? ClockIn { get; set; }
    public DateTime? ClockOut { get; set; }
    public TimeSpan? WorkHours { get; set; }
    public AttendanceStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty; // Office, Remote, WFH
}

/// <summary>
/// Represents a timesheet entry
/// </summary>
public class Timesheet
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime WeekStart { get; set; }
    public DateTime WeekEnd { get; set; }
    public List<TimesheetEntry> Entries { get; set; } = new();
    public TimeSpan TotalHours { get; set; }
    public TimesheetStatus Status { get; set; }
    public DateTime? SubmittedDate { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// Represents a single day entry in a timesheet
/// </summary>
public class TimesheetEntry
{
    public DateTime Date { get; set; }
    public TimeSpan Hours { get; set; }
    public string Project { get; set; } = string.Empty;
    public string Task { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Represents a holiday in the calendar
/// </summary>
public class Holiday
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsOptional { get; set; }
    public string Country { get; set; } = string.Empty;
}

/// <summary>
/// Represents overtime record
/// </summary>
public class OvertimeRecord
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Hours { get; set; }
    public string Reason { get; set; } = string.Empty;
    public OvertimeStatus Status { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}

// Enums
public enum AttendanceStatus
{
    Present,
    Absent,
    Late,
    HalfDay,
    OnLeave,
    Holiday,
    Weekend,
    WorkFromHome
}

public enum TimesheetStatus
{
    Draft,
    Submitted,
    Approved,
    Rejected,
    Revised
}

public enum OvertimeStatus
{
    Pending,
    Approved,
    Rejected
}
