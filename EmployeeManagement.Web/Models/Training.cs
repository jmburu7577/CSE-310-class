namespace EmployeeManagement.Web.Models;

/// <summary>
/// Represents a training course
/// </summary>
public class TrainingCourse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Instructor { get; set; } = string.Empty;
    public int DurationHours { get; set; }
    public CourseType Type { get; set; }
    public string Level { get; set; } = string.Empty; // Beginner, Intermediate, Advanced
    public decimal Cost { get; set; }
    public int MaxParticipants { get; set; }
    public List<string> Prerequisites { get; set; } = new();
    public CourseStatus Status { get; set; }
}

/// <summary>
/// Represents a training schedule
/// </summary>
public class TrainingSchedule
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Instructor { get; set; } = string.Empty;
    public List<int> EnrolledEmployees { get; set; } = new();
    public int AvailableSeats { get; set; }
    public ScheduleStatus Status { get; set; }
}

/// <summary>
/// Represents an employee's training enrollment
/// </summary>
public class TrainingEnrollment
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int ScheduleId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public EnrollmentStatus Status { get; set; }
    public DateTime? CompletionDate { get; set; }
    public int? Score { get; set; }
    public string Feedback { get; set; } = string.Empty;
    public bool CertificateIssued { get; set; }
}

/// <summary>
/// Represents a certification
/// </summary>
public class Certification
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IssuingOrganization { get; set; } = string.Empty;
    public string CertificationNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string CertificateUrl { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public CertificationStatus Status { get; set; }
}

/// <summary>
/// Represents an employee skill
/// </summary>
public class EmployeeSkill
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Technical, Soft Skills, Language, etc.
    public SkillLevel Proficiency { get; set; }
    public int YearsOfExperience { get; set; }
    public DateTime LastUsed { get; set; }
    public bool IsPrimary { get; set; }
    public string VerifiedBy { get; set; } = string.Empty;
}

/// <summary>
/// Represents a skill gap analysis
/// </summary>
public class SkillGapAnalysis
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string Position { get; set; } = string.Empty;
    public DateTime AnalysisDate { get; set; }
    public List<SkillGap> Gaps { get; set; } = new();
    public List<string> RecommendedTrainings { get; set; } = new();
    public string AnalyzedBy { get; set; } = string.Empty;
}

/// <summary>
/// Represents a skill gap
/// </summary>
public class SkillGap
{
    public string SkillName { get; set; } = string.Empty;
    public SkillLevel RequiredLevel { get; set; }
    public SkillLevel CurrentLevel { get; set; }
    public string Priority { get; set; } = string.Empty; // High, Medium, Low
}

// Enums
public enum CourseType
{
    InPerson,
    Online,
    Hybrid,
    SelfPaced,
    Workshop,
    Seminar
}

public enum CourseStatus
{
    Active,
    Inactive,
    Archived
}

public enum ScheduleStatus
{
    Scheduled,
    InProgress,
    Completed,
    Cancelled,
    Postponed
}

public enum EnrollmentStatus
{
    Enrolled,
    InProgress,
    Completed,
    Dropped,
    Failed,
    Passed
}

public enum CertificationStatus
{
    Active,
    Expired,
    Renewed,
    Revoked
}

public enum SkillLevel
{
    Beginner = 1,
    Intermediate = 2,
    Advanced = 3,
    Expert = 4
}
