using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

/// <summary>
/// Interface for payslip management operations
/// </summary>
public interface IPayslipService
{
    Task<IEnumerable<Payslip>> GetAllPayslipsAsync();
    Task<Payslip?> GetPayslipByIdAsync(int id);
    Task<IEnumerable<Payslip>> GetPayslipsByEmployeeIdAsync(int employeeId);
    Task<PayslipGenerationResult> GeneratePayslipAsync(int employeeId, GeneratePayslipRequest request);
    Task<PayslipGenerationResult> GenerateBulkPayslipsAsync(GeneratePayslipRequest request);
    Task<byte[]> GeneratePayslipPdfAsync(int payslipId);
    Task<bool> MarkPayslipAsViewedAsync(int payslipId);
    Task<bool> DeletePayslipAsync(int id);
}
