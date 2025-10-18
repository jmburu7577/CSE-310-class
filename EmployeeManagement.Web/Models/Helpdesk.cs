namespace EmployeeManagement.Web.Models;

/// <summary>
/// Represents an HR helpdesk ticket
/// </summary>
public class HRTicket
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketCategory Category { get; set; }
    public TicketPriority Priority { get; set; }
    public TicketStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ResolvedDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public string AssignedTo { get; set; } = string.Empty;
    public List<TicketComment> Comments { get; set; } = new();
    public List<string> Attachments { get; set; } = new();
}

/// <summary>
/// Represents a comment on a ticket
/// </summary>
public class TicketComment
{
    public int Id { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public bool IsInternal { get; set; } // Only visible to HR staff
}

/// <summary>
/// Represents a knowledge base article
/// </summary>
public class KnowledgeBaseArticle
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public int ViewCount { get; set; }
    public bool IsPublished { get; set; }
}

// Enums
public enum TicketCategory
{
    Payroll,
    Benefits,
    Leave,
    Performance,
    Training,
    IT,
    Workplace,
    Other
}

public enum TicketPriority
{
    Low,
    Medium,
    High,
    Urgent
}

public enum TicketStatus
{
    Open,
    InProgress,
    Waiting,
    Resolved,
    Closed,
    Cancelled
}
