using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

public interface IAttendanceService
{
    // Attendance Records
    Task<List<AttendanceRecord>> GetAllAttendanceAsync();
    Task<AttendanceRecord?> GetAttendanceByIdAsync(int id);
    Task<List<AttendanceRecord>> GetEmployeeAttendanceAsync(int employeeId, DateTime? startDate = null, DateTime? endDate = null);
    Task<AttendanceRecord> ClockInAsync(int employeeId, string location);
    Task<AttendanceRecord?> ClockOutAsync(int employeeId);
    Task<AttendanceRecord> CreateAttendanceRecordAsync(AttendanceRecord record);
    
    // Timesheets
    Task<List<Timesheet>> GetAllTimesheetsAsync();
    Task<Timesheet?> GetTimesheetByIdAsync(int id);
    Task<List<Timesheet>> GetEmployeeTimesheetsAsync(int employeeId);
    Task<Timesheet> CreateTimesheetAsync(Timesheet timesheet);
    Task<Timesheet?> SubmitTimesheetAsync(int id);
    Task<Timesheet?> ApproveTimesheetAsync(int id, string approvedBy);
    Task<Timesheet?> RejectTimesheetAsync(int id, string notes);
    
    // Holidays
    Task<List<Holiday>> GetAllHolidaysAsync();
    Task<Holiday> CreateHolidayAsync(Holiday holiday);
    Task<bool> DeleteHolidayAsync(int id);
    
    // Overtime
    Task<List<OvertimeRecord>> GetOvertimeRecordsAsync(int? employeeId = null);
    Task<OvertimeRecord> CreateOvertimeRecordAsync(OvertimeRecord record);
    Task<OvertimeRecord?> ApproveOvertimeAsync(int id, string approvedBy);
    
    // Statistics
    Task<object> GetAttendanceStatsAsync(DateTime? startDate = null, DateTime? endDate = null);
}
