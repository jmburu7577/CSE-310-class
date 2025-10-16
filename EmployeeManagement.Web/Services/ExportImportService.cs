using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

/// <summary>
/// Service for handling export and import of employee data
/// </summary>
public class ExportImportService : IExportImportService
{
    private readonly ILogger<ExportImportService> _logger;

    public ExportImportService(ILogger<ExportImportService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Export employees to CSV format
    /// </summary>
    public async Task<byte[]> ExportToCsvAsync(IEnumerable<Employee> employees)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            // Write CSV records
            await csv.WriteRecordsAsync(employees.Select(e => new
            {
                e.Id,
                e.FirstName,
                e.LastName,
                e.Age,
                e.Department,
                e.Salary,
                Street = e.Address.Street,
                City = e.Address.City,
                State = e.Address.State,
                Zip = e.Address.Zip
            }));

            await writer.FlushAsync();
            return memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting to CSV");
            throw;
        }
    }

    /// <summary>
    /// Export employees to Excel format (XLSX)
    /// </summary>
    public async Task<byte[]> ExportToExcelAsync(IEnumerable<Employee> employees)
    {
        try
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Employees");

            // Headers
            worksheet.Cell(1, 1).Value = "Employee ID";
            worksheet.Cell(1, 2).Value = "First Name";
            worksheet.Cell(1, 3).Value = "Last Name";
            worksheet.Cell(1, 4).Value = "Age";
            worksheet.Cell(1, 5).Value = "Department";
            worksheet.Cell(1, 6).Value = "Salary";
            worksheet.Cell(1, 7).Value = "Street";
            worksheet.Cell(1, 8).Value = "City";
            worksheet.Cell(1, 9).Value = "State";
            worksheet.Cell(1, 10).Value = "ZIP";

            // Style headers
            var headerRange = worksheet.Range(1, 1, 1, 10);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Data
            int row = 2;
            foreach (var employee in employees)
            {
                worksheet.Cell(row, 1).Value = employee.Id;
                worksheet.Cell(row, 2).Value = employee.FirstName;
                worksheet.Cell(row, 3).Value = employee.LastName;
                worksheet.Cell(row, 4).Value = employee.Age;
                worksheet.Cell(row, 5).Value = employee.Department;
                worksheet.Cell(row, 6).Value = employee.Salary;
                worksheet.Cell(row, 7).Value = employee.Address.Street;
                worksheet.Cell(row, 8).Value = employee.Address.City;
                worksheet.Cell(row, 9).Value = employee.Address.State;
                worksheet.Cell(row, 10).Value = employee.Address.Zip;
                row++;
            }

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            // Save to memory stream
            using var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            await Task.CompletedTask;
            return memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting to Excel");
            throw;
        }
    }

    /// <summary>
    /// Export employees to PDF format
    /// </summary>
    public async Task<byte[]> ExportToPdfAsync(IEnumerable<Employee> employees)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            var document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
            var writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            // Title
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            var title = new Paragraph("Employee Report\n\n", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            document.Add(title);

            // Date
            var dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            var date = new Paragraph($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n\n", dateFont)
            {
                Alignment = Element.ALIGN_RIGHT
            };
            document.Add(date);

            // Table
            var table = new PdfPTable(10)
            {
                WidthPercentage = 100
            };

            // Set column widths
            table.SetWidths(new float[] { 8f, 12f, 12f, 6f, 12f, 10f, 15f, 10f, 8f, 8f });

            // Header cells
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.White);
            var headerBg = new BaseColor(68, 114, 196);

            AddTableHeader(table, "ID", headerFont, headerBg);
            AddTableHeader(table, "First Name", headerFont, headerBg);
            AddTableHeader(table, "Last Name", headerFont, headerBg);
            AddTableHeader(table, "Age", headerFont, headerBg);
            AddTableHeader(table, "Department", headerFont, headerBg);
            AddTableHeader(table, "Salary", headerFont, headerBg);
            AddTableHeader(table, "Street", headerFont, headerBg);
            AddTableHeader(table, "City", headerFont, headerBg);
            AddTableHeader(table, "State", headerFont, headerBg);
            AddTableHeader(table, "ZIP", headerFont, headerBg);

            // Data cells
            var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
            foreach (var emp in employees)
            {
                AddTableCell(table, emp.Id.ToString(), cellFont);
                AddTableCell(table, emp.FirstName, cellFont);
                AddTableCell(table, emp.LastName, cellFont);
                AddTableCell(table, emp.Age.ToString(), cellFont);
                AddTableCell(table, emp.Department, cellFont);
                AddTableCell(table, $"${emp.Salary:N2}", cellFont);
                AddTableCell(table, emp.Address.Street, cellFont);
                AddTableCell(table, emp.Address.City, cellFont);
                AddTableCell(table, emp.Address.State, cellFont);
                AddTableCell(table, emp.Address.Zip, cellFont);
            }

            document.Add(table);

            // Summary
            var summary = new Paragraph($"\n\nTotal Employees: {employees.Count()}", dateFont);
            document.Add(summary);

            document.Close();
            writer.Close();

            await Task.CompletedTask;
            return memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting to PDF");
            throw;
        }
    }

    /// <summary>
    /// Import employees from CSV file
    /// </summary>
    public async Task<ImportResult> ImportFromCsvAsync(Stream csvStream)
    {
        var result = new ImportResult();
        var errors = new List<string>();
        var importedEmployees = new List<Employee>();

        try
        {
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            });

            var records = csv.GetRecords<CsvEmployeeRecord>().ToList();
            result.TotalRecords = records.Count;

            for (int i = 0; i < records.Count; i++)
            {
                var record = records[i];
                try
                {
                    // Validate record
                    if (string.IsNullOrWhiteSpace(record.FirstName) || string.IsNullOrWhiteSpace(record.LastName))
                    {
                        errors.Add($"Row {i + 2}: First name and last name are required");
                        result.FailedImports++;
                        continue;
                    }

                    if (record.Age < 18 || record.Age > 100)
                    {
                        errors.Add($"Row {i + 2}: Age must be between 18 and 100");
                        result.FailedImports++;
                        continue;
                    }

                    if (record.Salary < 0)
                    {
                        errors.Add($"Row {i + 2}: Salary must be positive");
                        result.FailedImports++;
                        continue;
                    }

                    // Create employee
                    var employee = new Employee
                    {
                        Id = record.Id,
                        FirstName = record.FirstName,
                        LastName = record.LastName,
                        Age = record.Age,
                        Department = record.Department ?? "Unassigned",
                        Salary = record.Salary,
                        Address = new Address
                        {
                            Street = record.Street ?? "",
                            City = record.City ?? "",
                            State = record.State ?? "",
                            Zip = record.Zip ?? ""
                        }
                    };

                    importedEmployees.Add(employee);
                    result.SuccessfulImports++;
                }
                catch (Exception ex)
                {
                    errors.Add($"Row {i + 2}: {ex.Message}");
                    result.FailedImports++;
                }
            }

            result.Success = result.SuccessfulImports > 0;
            result.Errors = errors;
            result.ImportedEmployees = importedEmployees;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing from CSV");
            result.Success = false;
            result.Errors.Add($"Import failed: {ex.Message}");
        }

        await Task.CompletedTask;
        return result;
    }

    private void AddTableHeader(PdfPTable table, string text, Font font, BaseColor bgColor)
    {
        var cell = new PdfPCell(new Phrase(text, font))
        {
            BackgroundColor = bgColor,
            HorizontalAlignment = Element.ALIGN_CENTER,
            VerticalAlignment = Element.ALIGN_MIDDLE,
            Padding = 5
        };
        table.AddCell(cell);
    }

    private void AddTableCell(PdfPTable table, string text, Font font)
    {
        var cell = new PdfPCell(new Phrase(text, font))
        {
            HorizontalAlignment = Element.ALIGN_LEFT,
            VerticalAlignment = Element.ALIGN_MIDDLE,
            Padding = 4
        };
        table.AddCell(cell);
    }
}

/// <summary>
/// CSV record mapping for import
/// </summary>
public class CsvEmployeeRecord
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string? Department { get; set; }
    public decimal Salary { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
}
