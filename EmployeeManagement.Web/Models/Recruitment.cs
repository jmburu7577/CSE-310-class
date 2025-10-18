namespace EmployeeManagement.Web.Models;

/// <summary>
/// Represents a job posting
/// </summary>
public class JobPosting
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public decimal SalaryMin { get; set; }
    public decimal SalaryMax { get; set; }
    public string Location { get; set; } = string.Empty;
    public JobType JobType { get; set; }
    public JobStatus Status { get; set; }
    public DateTime PostedDate { get; set; }
    public DateTime? ClosingDate { get; set; }
    public string PostedBy { get; set; } = string.Empty;
    public int Positions { get; set; }
}

/// <summary>
/// Represents a job application
/// </summary>
public class JobApplication
{
    public int Id { get; set; }
    public int JobPostingId { get; set; }
    public string ApplicantName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string ResumePath { get; set; } = string.Empty;
    public string CoverLetter { get; set; } = string.Empty;
    public ApplicationStatus Status { get; set; }
    public DateTime AppliedDate { get; set; }
    public string ReviewedBy { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// Represents an interview schedule
/// </summary>
public class Interview
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public string ApplicantName { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string InterviewType { get; set; } = string.Empty; // Phone, Video, In-person
    public string Interviewer { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public InterviewStatus Status { get; set; }
    public string Feedback { get; set; } = string.Empty;
    public int? Rating { get; set; } // 1-5
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// Represents an offer letter
/// </summary>
public class OfferLetter
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public string CandidateName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public decimal SalaryOffered { get; set; }
    public DateTime JoinDate { get; set; }
    public DateTime OfferDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public OfferStatus Status { get; set; }
    public string Benefits { get; set; } = string.Empty;
    public string Terms { get; set; } = string.Empty;
    public string GeneratedBy { get; set; } = string.Empty;
}

/// <summary>
/// Represents an onboarding checklist item
/// </summary>
public class OnboardingTask
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // HR, IT, Admin, etc.
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string CompletedBy { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

// Enums
public enum JobType
{
    FullTime,
    PartTime,
    Contract,
    Internship,
    Temporary
}

public enum JobStatus
{
    Draft,
    Open,
    Closed,
    OnHold,
    Cancelled
}

public enum ApplicationStatus
{
    Submitted,
    Screening,
    Shortlisted,
    Interview,
    Offered,
    Accepted,
    Rejected,
    Withdrawn
}

public enum InterviewStatus
{
    Scheduled,
    Completed,
    Cancelled,
    Rescheduled,
    NoShow
}

public enum OfferStatus
{
    Draft,
    Sent,
    Accepted,
    Rejected,
    Expired,
    Withdrawn
}
