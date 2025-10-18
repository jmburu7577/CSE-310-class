namespace EmployeeManagement.Web.Models;

/// <summary>
/// Represents an employee resignation
/// </summary>
public class Resignation
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime SubmissionDate { get; set; }
    public DateTime LastWorkingDay { get; set; }
    public string Reason { get; set; } = string.Empty;
    public ResignationType Type { get; set; }
    public ResignationStatus Status { get; set; }
    public int NoticePeriodDays { get; set; }
    public bool IsServed { get; set; }
    public string Comments { get; set; } = string.Empty;
    public string ProcessedBy { get; set; } = string.Empty;
}

/// <summary>
/// Represents a termination record
/// </summary>
public class Termination
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime TerminationDate { get; set; }
    public TerminationType Type { get; set; }
    public string Reason { get; set; } = string.Empty;
    public bool IsEligibleForRehire { get; set; }
    public string InitiatedBy { get; set; } = string.Empty;
    public DateTime? LastPaymentDate { get; set; }
    public decimal? SeverancePay { get; set; }
}

/// <summary>
/// Represents an exit interview
/// </summary>
public class ExitInterview
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime InterviewDate { get; set; }
    public string InterviewedBy { get; set; } = string.Empty;
    public List<ExitInterviewQuestion> Questions { get; set; } = new();
    public int? OverallSatisfaction { get; set; } // 1-5
    public bool WouldRecommendCompany { get; set; }
    public bool WouldConsiderReturning { get; set; }
    public string AdditionalFeedback { get; set; } = string.Empty;
    public ExitInterviewStatus Status { get; set; }
}

/// <summary>
/// Represents an exit interview question and response
/// </summary>
public class ExitInterviewQuestion
{
    public string Question { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
    public int? Rating { get; set; }
}

/// <summary>
/// Represents an exit clearance checklist
/// </summary>
public class ExitClearance
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime InitiatedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public List<ClearanceItem> Items { get; set; } = new();
    public bool IsFullyCleared { get; set; }
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// Represents a single clearance item
/// </summary>
public class ClearanceItem
{
    public string Department { get; set; } = string.Empty;
    public string ItemDescription { get; set; } = string.Empty;
    public bool IsCleared { get; set; }
    public DateTime? ClearedDate { get; set; }
    public string ClearedBy { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// Represents final settlement details
/// </summary>
public class FinalSettlement
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime CalculationDate { get; set; }
    public decimal OutstandingSalary { get; set; }
    public decimal UnusedLeaveEncashment { get; set; }
    public decimal Bonus { get; set; }
    public decimal Deductions { get; set; }
    public decimal TotalPayable { get; set; }
    public DateTime? PaymentDate { get; set; }
    public SettlementStatus Status { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public string ProcessedBy { get; set; } = string.Empty;
}

// Enums
public enum ResignationType
{
    Voluntary,
    Retirement,
    Mutual
}

public enum ResignationStatus
{
    Submitted,
    UnderReview,
    Accepted,
    Withdrawn,
    CounterOfferMade,
    CounterOfferAccepted,
    CounterOfferRejected
}

public enum TerminationType
{
    Layoff,
    PerformanceIssue,
    Misconduct,
    Redundancy,
    ContractEnd,
    Other
}

public enum ExitInterviewStatus
{
    Scheduled,
    Completed,
    Declined,
    Cancelled
}

public enum SettlementStatus
{
    Pending,
    Calculated,
    Approved,
    Paid,
    Disputed
}
