using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Services;

namespace EmployeeManagement.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RecruitmentController : ControllerBase
{
    private readonly IRecruitmentService _service;

    public RecruitmentController(IRecruitmentService service) => _service = service;

    [HttpGet("jobs")]
    public async Task<ActionResult<List<JobPosting>>> GetAllJobs() => Ok(await _service.GetAllJobPostingsAsync());

    [HttpGet("jobs/{id}")]
    public async Task<ActionResult<JobPosting>> GetJob(int id)
    {
        var job = await _service.GetJobPostingByIdAsync(id);
        return job == null ? NotFound() : Ok(job);
    }

    [HttpPost("jobs")]
    public async Task<ActionResult<JobPosting>> CreateJob(JobPosting job) => 
        Ok(await _service.CreateJobPostingAsync(job));

    [HttpPut("jobs/{id}")]
    public async Task<ActionResult<JobPosting>> UpdateJob(int id, JobPosting job)
    {
        var updated = await _service.UpdateJobPostingAsync(id, job);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("jobs/{id}")]
    public async Task<IActionResult> DeleteJob(int id) => 
        await _service.DeleteJobPostingAsync(id) ? NoContent() : NotFound();

    [HttpGet("applications")]
    public async Task<ActionResult<List<JobApplication>>> GetAllApplications() => 
        Ok(await _service.GetAllApplicationsAsync());

    [HttpGet("applications/{id}")]
    public async Task<ActionResult<JobApplication>> GetApplication(int id)
    {
        var app = await _service.GetApplicationByIdAsync(id);
        return app == null ? NotFound() : Ok(app);
    }

    [HttpPost("applications")]
    public async Task<ActionResult<JobApplication>> CreateApplication(JobApplication app) => 
        Ok(await _service.CreateApplicationAsync(app));

    [HttpPut("applications/{id}/status")]
    public async Task<ActionResult<JobApplication>> UpdateApplicationStatus(int id, 
        ApplicationStatus status, string notes)
    {
        var updated = await _service.UpdateApplicationStatusAsync(id, status, notes);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("interviews")]
    public async Task<ActionResult<List<Interview>>> GetAllInterviews() => 
        Ok(await _service.GetAllInterviewsAsync());

    [HttpPost("interviews")]
    public async Task<ActionResult<Interview>> ScheduleInterview(Interview interview) => 
        Ok(await _service.ScheduleInterviewAsync(interview));

    [HttpPut("interviews/{id}")]
    public async Task<ActionResult<Interview>> UpdateInterview(int id, Interview interview)
    {
        var updated = await _service.UpdateInterviewAsync(id, interview);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("offers")]
    public async Task<ActionResult<List<OfferLetter>>> GetAllOffers() => 
        Ok(await _service.GetAllOfferLettersAsync());

    [HttpPost("offers")]
    public async Task<ActionResult<OfferLetter>> CreateOffer(OfferLetter offer) => 
        Ok(await _service.CreateOfferLetterAsync(offer));

    [HttpPut("offers/{id}/status")]
    public async Task<ActionResult<OfferLetter>> UpdateOfferStatus(int id, OfferStatus status)
    {
        var updated = await _service.UpdateOfferStatusAsync(id, status);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("onboarding/employee/{employeeId}")]
    public async Task<ActionResult<List<OnboardingTask>>> GetOnboardingTasks(int employeeId) => 
        Ok(await _service.GetOnboardingTasksByEmployeeIdAsync(employeeId));

    [HttpPost("onboarding")]
    public async Task<ActionResult<OnboardingTask>> CreateOnboardingTask(OnboardingTask task) => 
        Ok(await _service.CreateOnboardingTaskAsync(task));

    [HttpPut("onboarding/{id}/complete")]
    public async Task<ActionResult<OnboardingTask>> CompleteTask(int id, string completedBy)
    {
        var updated = await _service.CompleteOnboardingTaskAsync(id, completedBy);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("stats")]
    public async Task<ActionResult<object>> GetStats() => Ok(await _service.GetRecruitmentStatsAsync());
}
