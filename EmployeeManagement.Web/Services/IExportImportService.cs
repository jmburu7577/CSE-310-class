using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

/// <summary>
/// Interface for export/import operations
/// </summary>
public interface IExportImportService
{
    // Export operations
    Task<byte[]> ExportToCsvAsync(IEnumerable<Employee> employees);
    Task<byte[]> ExportToExcelAsync(IEnumerable<Employee> employees);
    Task<byte[]> ExportToPdfAsync(IEnumerable<Employee> employees);
    
    // Import operations
    Task<ImportResult> ImportFromCsvAsync(Stream csvStream);
}

/// <summary>
/// Result of import operation
/// </summary>
public class ImportResult
{
    public bool Success { get; set; }
    public int TotalRecords { get; set; }
    public int SuccessfulImports { get; set; }
    public int FailedImports { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<Employee> ImportedEmployees { get; set; } = new();
}
