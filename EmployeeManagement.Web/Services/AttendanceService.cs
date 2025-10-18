using EmployeeManagement.Web.Models;
using System.Text.Json;

namespace EmployeeManagement.Web.Services;

public class AttendanceService : IAttendanceService
{
    private readonly string _attendanceFile = "Data/attendance.json";
    private readonly string _timesheetsFile = "Data/timesheets.json";
    private readonly string _holidaysFile = "Data/holidays.json";
    private readonly string _overtimeFile = "Data/overtime.json";
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public AttendanceService()
    {
        Directory.CreateDirectory("Data");
    }

    // Attendance Records
    public async Task<List<AttendanceRecord>> GetAllAttendanceAsync()
    {
        return await ReadFromFileAsync<AttendanceRecord>(_attendanceFile);
    }

    public async Task<AttendanceRecord?> GetAttendanceByIdAsync(int id)
    {
        var records = await GetAllAttendanceAsync();
        return records.FirstOrDefault(r => r.Id == id);
    }

    public async Task<List<AttendanceRecord>> GetEmployeeAttendanceAsync(int employeeId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var records = await GetAllAttendanceAsync();
        var filtered = records.Where(r => r.EmployeeId == employeeId);

        if (startDate.HasValue)
            filtered = filtered.Where(r => r.Date >= startDate.Value);
        if (endDate.HasValue)
            filtered = filtered.Where(r => r.Date <= endDate.Value);

        return filtered.OrderByDescending(r => r.Date).ToList();
    }

    public async Task<AttendanceRecord> ClockInAsync(int employeeId, string location)
    {
        var records = await GetAllAttendanceAsync();
        var today = DateTime.UtcNow.Date;
        
        var existingRecord = records.FirstOrDefault(r => r.EmployeeId == employeeId && r.Date.Date == today);
        
        if (existingRecord != null && existingRecord.ClockIn.HasValue)
        {
            throw new InvalidOperationException("Already clocked in today");
        }

        if (existingRecord == null)
        {
            existingRecord = new AttendanceRecord
            {
                Id = records.Any() ? records.Max(r => r.Id) + 1 : 1,
                EmployeeId = employeeId,
                Date = today,
                Location = location,
                Status = AttendanceStatus.Present
            };
            records.Add(existingRecord);
        }

        existingRecord.ClockIn = DateTime.UtcNow;
        await SaveToFileAsync(_attendanceFile, records);
        return existingRecord;
    }

    public async Task<AttendanceRecord?> ClockOutAsync(int employeeId)
    {
        var records = await GetAllAttendanceAsync();
        var today = DateTime.UtcNow.Date;
        
        var record = records.FirstOrDefault(r => r.EmployeeId == employeeId && r.Date.Date == today);
        
        if (record == null || !record.ClockIn.HasValue)
        {
            throw new InvalidOperationException("No clock-in record found for today");
        }

        if (record.ClockOut.HasValue)
        {
            throw new InvalidOperationException("Already clocked out today");
        }

        record.ClockOut = DateTime.UtcNow;
        record.WorkHours = record.ClockOut.Value - record.ClockIn.Value;
        
        await SaveToFileAsync(_attendanceFile, records);
        return record;
    }

    public async Task<AttendanceRecord> CreateAttendanceRecordAsync(AttendanceRecord record)
    {
        var records = await GetAllAttendanceAsync();
        record.Id = records.Any() ? records.Max(r => r.Id) + 1 : 1;
        records.Add(record);
        await SaveToFileAsync(_attendanceFile, records);
        return record;
    }

    // Timesheets
    public async Task<List<Timesheet>> GetAllTimesheetsAsync()
    {
        return await ReadFromFileAsync<Timesheet>(_timesheetsFile);
    }

    public async Task<Timesheet?> GetTimesheetByIdAsync(int id)
    {
        var timesheets = await GetAllTimesheetsAsync();
        return timesheets.FirstOrDefault(t => t.Id == id);
    }

    public async Task<List<Timesheet>> GetEmployeeTimesheetsAsync(int employeeId)
    {
        var timesheets = await GetAllTimesheetsAsync();
        return timesheets.Where(t => t.EmployeeId == employeeId).OrderByDescending(t => t.WeekStart).ToList();
    }

    public async Task<Timesheet> CreateTimesheetAsync(Timesheet timesheet)
    {
        var timesheets = await GetAllTimesheetsAsync();
        timesheet.Id = timesheets.Any() ? timesheets.Max(t => t.Id) + 1 : 1;
        timesheet.Status = TimesheetStatus.Draft;
        timesheet.TotalHours = TimeSpan.FromHours(timesheet.Entries.Sum(e => e.Hours.TotalHours));
        timesheets.Add(timesheet);
        await SaveToFileAsync(_timesheetsFile, timesheets);
        return timesheet;
    }

    public async Task<Timesheet?> SubmitTimesheetAsync(int id)
    {
        var timesheets = await GetAllTimesheetsAsync();
        var timesheet = timesheets.FirstOrDefault(t => t.Id == id);
        if (timesheet == null) return null;

        timesheet.Status = TimesheetStatus.Submitted;
        timesheet.SubmittedDate = DateTime.UtcNow;
        await SaveToFileAsync(_timesheetsFile, timesheets);
        return timesheet;
    }

    public async Task<Timesheet?> ApproveTimesheetAsync(int id, string approvedBy)
    {
        var timesheets = await GetAllTimesheetsAsync();
        var timesheet = timesheets.FirstOrDefault(t => t.Id == id);
        if (timesheet == null) return null;

        timesheet.Status = TimesheetStatus.Approved;
        timesheet.ApprovedDate = DateTime.UtcNow;
        timesheet.ApprovedBy = approvedBy;
        await SaveToFileAsync(_timesheetsFile, timesheets);
        return timesheet;
    }

    public async Task<Timesheet?> RejectTimesheetAsync(int id, string notes)
    {
        var timesheets = await GetAllTimesheetsAsync();
        var timesheet = timesheets.FirstOrDefault(t => t.Id == id);
        if (timesheet == null) return null;

        timesheet.Status = TimesheetStatus.Rejected;
        timesheet.Notes = notes;
        await SaveToFileAsync(_timesheetsFile, timesheets);
        return timesheet;
    }

    // Holidays
    public async Task<List<Holiday>> GetAllHolidaysAsync()
    {
        return await ReadFromFileAsync<Holiday>(_holidaysFile);
    }

    public async Task<Holiday> CreateHolidayAsync(Holiday holiday)
    {
        var holidays = await ReadFromFileAsync<Holiday>(_holidaysFile);
        holiday.Id = holidays.Any() ? holidays.Max(h => h.Id) + 1 : 1;
        holidays.Add(holiday);
        await SaveToFileAsync(_holidaysFile, holidays);
        return holiday;
    }

    public async Task<bool> DeleteHolidayAsync(int id)
    {
        var holidays = await ReadFromFileAsync<Holiday>(_holidaysFile);
        var holiday = holidays.FirstOrDefault(h => h.Id == id);
        if (holiday == null) return false;

        holidays.Remove(holiday);
        await SaveToFileAsync(_holidaysFile, holidays);
        return true;
    }

    // Overtime
    public async Task<List<OvertimeRecord>> GetOvertimeRecordsAsync(int? employeeId = null)
    {
        var records = await ReadFromFileAsync<OvertimeRecord>(_overtimeFile);
        if (employeeId.HasValue)
            return records.Where(r => r.EmployeeId == employeeId.Value).ToList();
        return records;
    }

    public async Task<OvertimeRecord> CreateOvertimeRecordAsync(OvertimeRecord record)
    {
        var records = await ReadFromFileAsync<OvertimeRecord>(_overtimeFile);
        record.Id = records.Any() ? records.Max(r => r.Id) + 1 : 1;
        record.Status = OvertimeStatus.Pending;
        records.Add(record);
        await SaveToFileAsync(_overtimeFile, records);
        return record;
    }

    public async Task<OvertimeRecord?> ApproveOvertimeAsync(int id, string approvedBy)
    {
        var records = await ReadFromFileAsync<OvertimeRecord>(_overtimeFile);
        var record = records.FirstOrDefault(r => r.Id == id);
        if (record == null) return null;

        record.Status = OvertimeStatus.Approved;
        record.ApprovedDate = DateTime.UtcNow;
        record.ApprovedBy = approvedBy;
        await SaveToFileAsync(_overtimeFile, records);
        return record;
    }

    // Statistics
    public async Task<object> GetAttendanceStatsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var records = await GetAllAttendanceAsync();
        
        if (startDate.HasValue)
            records = records.Where(r => r.Date >= startDate.Value).ToList();
        if (endDate.HasValue)
            records = records.Where(r => r.Date <= endDate.Value).ToList();

        return new
        {
            TotalRecords = records.Count,
            PresentCount = records.Count(r => r.Status == AttendanceStatus.Present),
            AbsentCount = records.Count(r => r.Status == AttendanceStatus.Absent),
            LateCount = records.Count(r => r.Status == AttendanceStatus.Late),
            WFHCount = records.Count(r => r.Status == AttendanceStatus.WorkFromHome),
            AverageWorkHours = records.Where(r => r.WorkHours.HasValue)
                .Average(r => r.WorkHours!.Value.TotalHours)
        };
    }

    // Helper Methods
    private async Task<List<T>> ReadFromFileAsync<T>(string filePath)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (!File.Exists(filePath))
                return new List<T>();

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task SaveToFileAsync<T>(string filePath, List<T> data)
    {
        await _semaphore.WaitAsync();
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(data, options);
            await File.WriteAllTextAsync(filePath, json);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
