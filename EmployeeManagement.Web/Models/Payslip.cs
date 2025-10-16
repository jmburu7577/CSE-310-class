namespace EmployeeManagement.Web.Models;

/// <summary>
/// Represents a payslip for an employee
/// </summary>
public class Payslip
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime PayPeriodStart { get; set; }
    public DateTime PayPeriodEnd { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal Tax { get; set; }
    public decimal Deductions { get; set; }
    public decimal Bonuses { get; set; }
    public decimal NetSalary { get; set; }
    public DateTime GeneratedDate { get; set; }
    public string? Notes { get; set; }
    public PayslipStatus Status { get; set; }
}

/// <summary>
/// Payslip status
/// </summary>
public enum PayslipStatus
{
    Draft,
    Generated,
    Sent,
    Viewed
}

/// <summary>
/// Request to generate payslips
/// </summary>
public class GeneratePayslipRequest
{
    public int? EmployeeId { get; set; }  // Null for all employees
    public DateTime PayPeriodStart { get; set; }
    public DateTime PayPeriodEnd { get; set; }
    public decimal? BonusAmount { get; set; }
    public decimal? AdditionalDeductions { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Payslip generation result
/// </summary>
public class PayslipGenerationResult
{
    public bool Success { get; set; }
    public int TotalGenerated { get; set; }
    public List<int> PayslipIds { get; set; } = new();
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Payslip with employee details for display
/// </summary>
public class PayslipWithEmployee
{
    public Payslip Payslip { get; set; } = null!;
    public Employee Employee { get; set; } = null!;
}
