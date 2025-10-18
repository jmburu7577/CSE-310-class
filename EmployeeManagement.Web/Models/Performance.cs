namespace EmployeeManagement.Web.Models;

/// <summary>
/// Represents a performance goal/KPI
/// </summary>
public class PerformanceGoal
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Individual, Team, Organizational
    public GoalType Type { get; set; }
    public decimal TargetValue { get; set; }
    public decimal CurrentValue { get; set; }
    public string Unit { get; set; } = string.Empty; // %, $, units, etc.
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
    public GoalStatus Status { get; set; }
    public int Weight { get; set; } // Percentage weight in overall performance
    public string SetBy { get; set; } = string.Empty;
}

/// <summary>
/// Represents a performance review
/// </summary>
public class PerformanceReview
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime ReviewDate { get; set; }
    public string ReviewPeriodStart { get; set; } = string.Empty;
    public string ReviewPeriodEnd { get; set; } = string.Empty;
    public ReviewType Type { get; set; }
    public string ReviewerId { get; set; } = string.Empty;
    public string ReviewerName { get; set; } = string.Empty;
    public List<ReviewCriteria> Criteria { get; set; } = new();
    public decimal OverallRating { get; set; }
    public string Strengths { get; set; } = string.Empty;
    public string AreasForImprovement { get; set; } = string.Empty;
    public string Comments { get; set; } = string.Empty;
    public ReviewStatus Status { get; set; }
}

/// <summary>
/// Represents a review criterion with rating
/// </summary>
public class ReviewCriteria
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Rating { get; set; } // 1-5
    public string Comments { get; set; } = string.Empty;
}

/// <summary>
/// Represents a 360-degree feedback
/// </summary>
public class Feedback360
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int ReviewCycle { get; set; }
    public string ReviewerType { get; set; } = string.Empty; // Manager, Peer, Subordinate, Self
    public string ReviewerId { get; set; } = string.Empty;
    public string ReviewerName { get; set; } = string.Empty;
    public List<FeedbackQuestion> Questions { get; set; } = new();
    public DateTime SubmittedDate { get; set; }
    public bool IsAnonymous { get; set; }
}

/// <summary>
/// Represents a feedback question and response
/// </summary>
public class FeedbackQuestion
{
    public string Question { get; set; } = string.Empty;
    public int? Rating { get; set; }
    public string Response { get; set; } = string.Empty;
}

/// <summary>
/// Represents an appraisal record
/// </summary>
public class Appraisal
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime AppraisalDate { get; set; }
    public string Period { get; set; } = string.Empty;
    public decimal CurrentSalary { get; set; }
    public decimal ProposedSalary { get; set; }
    public decimal Increment { get; set; }
    public string IncrementPercentage { get; set; } = string.Empty;
    public string Bonus { get; set; } = string.Empty;
    public PromotionDetails? Promotion { get; set; }
    public string Justification { get; set; } = string.Empty;
    public AppraisalStatus Status { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}

/// <summary>
/// Represents promotion details
/// </summary>
public class PromotionDetails
{
    public string CurrentDesignation { get; set; } = string.Empty;
    public string NewDesignation { get; set; } = string.Empty;
    public string CurrentLevel { get; set; } = string.Empty;
    public string NewLevel { get; set; } = string.Empty;
}

// Enums
public enum GoalType
{
    KPI,
    Objective,
    Development,
    Project
}

public enum GoalStatus
{
    NotStarted,
    InProgress,
    OnTrack,
    AtRisk,
    Completed,
    Cancelled
}

public enum ReviewType
{
    Quarterly,
    MidYear,
    Annual,
    Probation,
    Project
}

public enum ReviewStatus
{
    Pending,
    InProgress,
    Completed,
    Acknowledged
}

public enum AppraisalStatus
{
    Draft,
    Submitted,
    UnderReview,
    Approved,
    Rejected,
    Implemented
}
