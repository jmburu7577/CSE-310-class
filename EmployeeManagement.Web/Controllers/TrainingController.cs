using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Services;

namespace EmployeeManagement.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TrainingController : ControllerBase
{
    private readonly ITrainingService _service;

    public TrainingController(ITrainingService service) => _service = service;

    [HttpGet("courses")]
    public async Task<ActionResult<List<TrainingCourse>>> GetAllCourses() => 
        Ok(await _service.GetAllCoursesAsync());

    [HttpPost("courses")]
    public async Task<ActionResult<TrainingCourse>> CreateCourse(TrainingCourse course) => 
        Ok(await _service.CreateCourseAsync(course));

    [HttpPut("courses/{id}")]
    public async Task<ActionResult<TrainingCourse>> UpdateCourse(int id, TrainingCourse course)
    {
        var updated = await _service.UpdateCourseAsync(id, course);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("schedules")]
    public async Task<ActionResult<List<TrainingSchedule>>> GetAllSchedules() => 
        Ok(await _service.GetAllSchedulesAsync());

    [HttpPost("schedules")]
    public async Task<ActionResult<TrainingSchedule>> CreateSchedule(TrainingSchedule schedule) => 
        Ok(await _service.CreateScheduleAsync(schedule));

    [HttpGet("enrollments")]
    public async Task<ActionResult<List<TrainingEnrollment>>> GetAllEnrollments() => 
        Ok(await _service.GetAllEnrollmentsAsync());

    [HttpGet("enrollments/employee/{employeeId}")]
    public async Task<ActionResult<List<TrainingEnrollment>>> GetEmployeeEnrollments(int employeeId) => 
        Ok(await _service.GetEmployeeEnrollmentsAsync(employeeId));

    [HttpPost("enrollments")]
    public async Task<ActionResult<TrainingEnrollment>> EnrollEmployee(TrainingEnrollment enrollment) => 
        Ok(await _service.EnrollEmployeeAsync(enrollment));

    [HttpPut("enrollments/{id}/complete")]
    public async Task<ActionResult<TrainingEnrollment>> CompleteTraining(int id, int score, string feedback)
    {
        var updated = await _service.CompleteTrainingAsync(id, score, feedback);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("certifications")]
    public async Task<ActionResult<List<Certification>>> GetAllCertifications() => 
        Ok(await _service.GetAllCertificationsAsync());

    [HttpGet("certifications/employee/{employeeId}")]
    public async Task<ActionResult<List<Certification>>> GetEmployeeCertifications(int employeeId) => 
        Ok(await _service.GetEmployeeCertificationsAsync(employeeId));

    [HttpPost("certifications")]
    public async Task<ActionResult<Certification>> AddCertification(Certification certification) => 
        Ok(await _service.AddCertificationAsync(certification));

    [HttpGet("skills/employee/{employeeId}")]
    public async Task<ActionResult<List<EmployeeSkill>>> GetEmployeeSkills(int employeeId) => 
        Ok(await _service.GetEmployeeSkillsAsync(employeeId));

    [HttpPost("skills")]
    public async Task<ActionResult<EmployeeSkill>> AddSkill(EmployeeSkill skill) => 
        Ok(await _service.AddSkillAsync(skill));

    [HttpGet("skill-gap/{employeeId}")]
    public async Task<ActionResult<SkillGapAnalysis>> GetSkillGapAnalysis(int employeeId)
    {
        var analysis = await _service.GetSkillGapAnalysisAsync(employeeId);
        return analysis == null ? NotFound() : Ok(analysis);
    }

    [HttpPost("skill-gap")]
    public async Task<ActionResult<SkillGapAnalysis>> CreateSkillGapAnalysis(SkillGapAnalysis analysis) => 
        Ok(await _service.CreateSkillGapAnalysisAsync(analysis));

    [HttpGet("stats")]
    public async Task<ActionResult<object>> GetStats() => Ok(await _service.GetTrainingStatsAsync());
}
