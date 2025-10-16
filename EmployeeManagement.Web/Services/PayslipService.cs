using System.Text.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;
using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

/// <summary>
/// Service for managing payslips with file persistence
/// </summary>
public class PayslipService : IPayslipService
{
    private readonly List<Payslip> _payslips = new();
    private readonly string _dataFile;
    private readonly SemaphoreSlim _fileLock = new(1, 1);
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<PayslipService> _logger;
    private int _nextId = 1;

    public PayslipService(IEmployeeService employeeService, ILogger<PayslipService> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
        _dataFile = Path.Combine(AppContext.BaseDirectory, "payslips.json");
        LoadFromFileAsync().Wait();
    }

    public async Task<IEnumerable<Payslip>> GetAllPayslipsAsync()
    {
        await Task.CompletedTask;
        return _payslips.OrderByDescending(p => p.GeneratedDate).ToList();
    }

    public async Task<Payslip?> GetPayslipByIdAsync(int id)
    {
        await Task.CompletedTask;
        return _payslips.FirstOrDefault(p => p.Id == id);
    }

    public async Task<IEnumerable<Payslip>> GetPayslipsByEmployeeIdAsync(int employeeId)
    {
        await Task.CompletedTask;
        return _payslips
            .Where(p => p.EmployeeId == employeeId)
            .OrderByDescending(p => p.GeneratedDate)
            .ToList();
    }

    public async Task<PayslipGenerationResult> GeneratePayslipAsync(int employeeId, GeneratePayslipRequest request)
    {
        var result = new PayslipGenerationResult();

        try
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
            if (employee == null)
            {
                result.Errors.Add($"Employee with ID {employeeId} not found");
                return result;
            }

            var payslip = CreatePayslip(employee, request);
            _payslips.Add(payslip);
            await SaveToFileAsync();

            result.Success = true;
            result.TotalGenerated = 1;
            result.PayslipIds.Add(payslip.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating payslip for employee {EmployeeId}", employeeId);
            result.Errors.Add($"Failed to generate payslip: {ex.Message}");
        }

        return result;
    }

    public async Task<PayslipGenerationResult> GenerateBulkPayslipsAsync(GeneratePayslipRequest request)
    {
        var result = new PayslipGenerationResult();

        try
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            
            foreach (var employee in employees)
            {
                try
                {
                    var payslip = CreatePayslip(employee, request);
                    _payslips.Add(payslip);
                    result.PayslipIds.Add(payslip.Id);
                    result.TotalGenerated++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating payslip for employee {EmployeeId}", employee.Id);
                    result.Errors.Add($"Employee {employee.Id}: {ex.Message}");
                }
            }

            await SaveToFileAsync();
            result.Success = result.TotalGenerated > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in bulk payslip generation");
            result.Errors.Add($"Bulk generation failed: {ex.Message}");
        }

        return result;
    }

    public async Task<byte[]> GeneratePayslipPdfAsync(int payslipId)
    {
        var payslip = await GetPayslipByIdAsync(payslipId);
        if (payslip == null)
        {
            throw new InvalidOperationException($"Payslip with ID {payslipId} not found");
        }

        var employee = await _employeeService.GetEmployeeByIdAsync(payslip.EmployeeId);
        if (employee == null)
        {
            throw new InvalidOperationException($"Employee with ID {payslip.EmployeeId} not found");
        }

        return GeneratePayslipPdf(payslip, employee);
    }

    public async Task<bool> MarkPayslipAsViewedAsync(int payslipId)
    {
        var payslip = await GetPayslipByIdAsync(payslipId);
        if (payslip == null)
        {
            return false;
        }

        if (payslip.Status == PayslipStatus.Generated || payslip.Status == PayslipStatus.Sent)
        {
            payslip.Status = PayslipStatus.Viewed;
            await SaveToFileAsync();
        }

        return true;
    }

    public async Task<bool> DeletePayslipAsync(int id)
    {
        var payslip = _payslips.FirstOrDefault(p => p.Id == id);
        if (payslip == null)
        {
            return false;
        }

        _payslips.Remove(payslip);
        await SaveToFileAsync();
        return true;
    }

    private Payslip CreatePayslip(Employee employee, GeneratePayslipRequest request)
    {
        // Calculate monthly salary (assuming annual salary in employee record)
        var monthlyGross = employee.Salary / 12;

        // Calculate tax (simple progressive tax example: 20% for illustration)
        var taxRate = 0.20m;
        var tax = monthlyGross * taxRate;

        // Add bonuses if specified
        var bonuses = request.BonusAmount ?? 0;

        // Add deductions if specified
        var deductions = request.AdditionalDeductions ?? 0;

        // Calculate net salary
        var netSalary = monthlyGross + bonuses - tax - deductions;

        var payslip = new Payslip
        {
            Id = _nextId++,
            EmployeeId = employee.Id,
            PayPeriodStart = request.PayPeriodStart,
            PayPeriodEnd = request.PayPeriodEnd,
            GrossSalary = monthlyGross,
            Tax = tax,
            Deductions = deductions,
            Bonuses = bonuses,
            NetSalary = netSalary,
            GeneratedDate = DateTime.UtcNow,
            Notes = request.Notes,
            Status = PayslipStatus.Generated
        };

        return payslip;
    }

    private byte[] GeneratePayslipPdf(Payslip payslip, Employee employee)
    {
        using var memoryStream = new MemoryStream();
        var document = new Document(PageSize.A4, 50, 50, 50, 50);
        var writer = PdfWriter.GetInstance(document, memoryStream);

        document.Open();

        // Company Header
        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.DarkGray);
        var title = new Paragraph("PAYSLIP", titleFont)
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 20
        };
        document.Add(title);

        // Company Info
        var companyFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
        var companyInfo = new Paragraph("Employee Management System\nHR Department\n\n", companyFont)
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 20
        };
        document.Add(companyInfo);

        // Payslip Info
        var infoTable = new PdfPTable(2) { WidthPercentage = 100, SpacingAfter = 20 };
        infoTable.SetWidths(new float[] { 1f, 1f });

        var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);
        var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

        // Left column - Employee Info
        AddInfoCell(infoTable, "Employee Information", headerFont, true);
        AddInfoCell(infoTable, "", normalFont, false);
        
        AddInfoCell(infoTable, $"Employee ID: {employee.Id}", normalFont, false);
        AddInfoCell(infoTable, $"Payslip ID: {payslip.Id}", normalFont, false);
        
        AddInfoCell(infoTable, $"Name: {employee.GetDisplayName()}", normalFont, false);
        AddInfoCell(infoTable, $"Generated: {payslip.GeneratedDate:yyyy-MM-dd}", normalFont, false);
        
        AddInfoCell(infoTable, $"Department: {employee.Department}", normalFont, false);
        AddInfoCell(infoTable, $"Period: {payslip.PayPeriodStart:MMM dd} - {payslip.PayPeriodEnd:MMM dd, yyyy}", normalFont, false);

        document.Add(infoTable);

        // Earnings and Deductions Table
        var salaryTable = new PdfPTable(2) { WidthPercentage = 100, SpacingAfter = 20 };
        salaryTable.SetWidths(new float[] { 3f, 1f });

        var tableBgColor = new BaseColor(68, 114, 196);
        var tableHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, BaseColor.White);

        AddSalaryHeader(salaryTable, "Description", tableHeaderFont, tableBgColor);
        AddSalaryHeader(salaryTable, "Amount", tableHeaderFont, tableBgColor);

        // Earnings
        AddSalaryRow(salaryTable, "Gross Salary", payslip.GrossSalary, normalFont, false);
        if (payslip.Bonuses > 0)
        {
            AddSalaryRow(salaryTable, "Bonuses", payslip.Bonuses, normalFont, false);
        }

        // Deductions
        AddSalaryRow(salaryTable, "Tax (20%)", payslip.Tax, normalFont, true);
        if (payslip.Deductions > 0)
        {
            AddSalaryRow(salaryTable, "Other Deductions", payslip.Deductions, normalFont, true);
        }

        // Net Salary
        var totalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
        AddSalaryRow(salaryTable, "NET SALARY", payslip.NetSalary, totalFont, false, new BaseColor(240, 240, 240));

        document.Add(salaryTable);

        // Notes
        if (!string.IsNullOrWhiteSpace(payslip.Notes))
        {
            var notesTitle = new Paragraph("Notes:", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))
            {
                SpacingAfter = 5
            };
            document.Add(notesTitle);

            var notes = new Paragraph(payslip.Notes, normalFont)
            {
                SpacingAfter = 20
            };
            document.Add(notes);
        }

        // Footer
        var footer = new Paragraph(
            "\n\nThis is a computer-generated payslip and does not require a signature.\n" +
            "For any queries, please contact the HR department.",
            FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 8, BaseColor.Gray))
        {
            Alignment = Element.ALIGN_CENTER
        };
        document.Add(footer);

        document.Close();
        writer.Close();

        return memoryStream.ToArray();
    }

    private void AddInfoCell(PdfPTable table, string text, Font font, bool isHeader)
    {
        var cell = new PdfPCell(new Phrase(text, font))
        {
            Border = Rectangle.NO_BORDER,
            Padding = 5
        };

        if (isHeader)
        {
            cell.BackgroundColor = new BaseColor(240, 240, 240);
        }

        table.AddCell(cell);
    }

    private void AddSalaryHeader(PdfPTable table, string text, Font font, BaseColor bgColor)
    {
        var cell = new PdfPCell(new Phrase(text, font))
        {
            BackgroundColor = bgColor,
            HorizontalAlignment = Element.ALIGN_LEFT,
            Padding = 8
        };
        table.AddCell(cell);
    }

    private void AddSalaryRow(PdfPTable table, string description, decimal amount, Font font, bool isDeduction, BaseColor? bgColor = null)
    {
        var descCell = new PdfPCell(new Phrase(description, font))
        {
            HorizontalAlignment = Element.ALIGN_LEFT,
            Padding = 6,
            BackgroundColor = bgColor ?? BaseColor.White
        };
        table.AddCell(descCell);

        var amountText = isDeduction ? $"- ${amount:N2}" : $"${amount:N2}";
        var amountCell = new PdfPCell(new Phrase(amountText, font))
        {
            HorizontalAlignment = Element.ALIGN_RIGHT,
            Padding = 6,
            BackgroundColor = bgColor ?? BaseColor.White
        };
        table.AddCell(amountCell);
    }

    private async Task SaveToFileAsync()
    {
        await _fileLock.WaitAsync();
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(_payslips, options);
            await File.WriteAllTextAsync(_dataFile, json);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private async Task LoadFromFileAsync()
    {
        if (!File.Exists(_dataFile))
        {
            return;
        }

        await _fileLock.WaitAsync();
        try
        {
            var json = await File.ReadAllTextAsync(_dataFile);
            var data = JsonSerializer.Deserialize<List<Payslip>>(json) ?? new List<Payslip>();
            _payslips.Clear();
            _payslips.AddRange(data);

            // Set next ID
            _nextId = _payslips.Any() ? _payslips.Max(p => p.Id) + 1 : 1;
        }
        finally
        {
            _fileLock.Release();
        }
    }
}
