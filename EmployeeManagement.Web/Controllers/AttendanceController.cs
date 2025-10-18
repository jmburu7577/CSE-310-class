using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Services;

namespace EmployeeManagement.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _service;

    public AttendanceController(IAttendanceService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<AttendanceRecord>>> GetAll() => 
        Ok(await _service.GetAllAttendanceAsync());

    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<List<AttendanceRecord>>> GetEmployeeAttendance(int employeeId, 
        DateTime? startDate = null, DateTime? endDate = null) => 
        Ok(await _service.GetEmployeeAttendanceAsync(employeeId, startDate, endDate));

    [HttpPost("clock-in")]
    public async Task<ActionResult<AttendanceRecord>> ClockIn(int employeeId, string location) => 
        Ok(await _service.ClockInAsync(employeeId, location));

    [HttpPost("clock-out")]
    public async Task<ActionResult<AttendanceRecord>> ClockOut(int employeeId)
    {
        var record = await _service.ClockOutAsync(employeeId);
        return record == null ? NotFound() : Ok(record);
    }

    [HttpGet("timesheets")]
    public async Task<ActionResult<List<Timesheet>>> GetAllTimesheets() => 
        Ok(await _service.GetAllTimesheetsAsync());

    [HttpGet("timesheets/employee/{employeeId}")]
    public async Task<ActionResult<List<Timesheet>>> GetEmployeeTimesheets(int employeeId) => 
        Ok(await _service.GetEmployeeTimesheetsAsync(employeeId));

    [HttpPost("timesheets")]
    public async Task<ActionResult<Timesheet>> CreateTimesheet(Timesheet timesheet) => 
        Ok(await _service.CreateTimesheetAsync(timesheet));

    [HttpPut("timesheets/{id}/submit")]
    public async Task<ActionResult<Timesheet>> SubmitTimesheet(int id)
    {
        var updated = await _service.SubmitTimesheetAsync(id);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpPut("timesheets/{id}/approve")]
    public async Task<ActionResult<Timesheet>> ApproveTimesheet(int id, string approvedBy)
    {
        var updated = await _service.ApproveTimesheetAsync(id, approvedBy);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("holidays")]
    public async Task<ActionResult<List<Holiday>>> GetHolidays() => 
        Ok(await _service.GetAllHolidaysAsync());

    [HttpPost("holidays")]
    public async Task<ActionResult<Holiday>> CreateHoliday(Holiday holiday) => 
        Ok(await _service.CreateHolidayAsync(holiday));

    [HttpGet("overtime")]
    public async Task<ActionResult<List<OvertimeRecord>>> GetOvertime(int? employeeId = null) => 
        Ok(await _service.GetOvertimeRecordsAsync(employeeId));

    [HttpPost("overtime")]
    public async Task<ActionResult<OvertimeRecord>> CreateOvertime(OvertimeRecord record) => 
        Ok(await _service.CreateOvertimeRecordAsync(record));

    [HttpPut("overtime/{id}/approve")]
    public async Task<ActionResult<OvertimeRecord>> ApproveOvertime(int id, string approvedBy)
    {
        var updated = await _service.ApproveOvertimeAsync(id, approvedBy);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("stats")]
    public async Task<ActionResult<object>> GetStats(DateTime? startDate = null, DateTime? endDate = null) => 
        Ok(await _service.GetAttendanceStatsAsync(startDate, endDate));
}
