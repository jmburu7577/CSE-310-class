using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Services;

namespace EmployeeManagement.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PerformanceController : ControllerBase
{
    private readonly IPerformanceService _service;

    public PerformanceController(IPerformanceService service) => _service = service;

    [HttpGet("goals")]
    public async Task<ActionResult<List<PerformanceGoal>>> GetAllGoals() => 
        Ok(await _service.GetAllGoalsAsync());

    [HttpGet("goals/employee/{employeeId}")]
    public async Task<ActionResult<List<PerformanceGoal>>> GetEmployeeGoals(int employeeId) => 
        Ok(await _service.GetEmployeeGoalsAsync(employeeId));

    [HttpPost("goals")]
    public async Task<ActionResult<PerformanceGoal>> CreateGoal(PerformanceGoal goal) => 
        Ok(await _service.CreateGoalAsync(goal));

    [HttpPut("goals/{id}")]
    public async Task<ActionResult<PerformanceGoal>> UpdateGoal(int id, PerformanceGoal goal)
    {
        var updated = await _service.UpdateGoalAsync(id, goal);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("reviews")]
    public async Task<ActionResult<List<PerformanceReview>>> GetAllReviews() => 
        Ok(await _service.GetAllReviewsAsync());

    [HttpGet("reviews/employee/{employeeId}")]
    public async Task<ActionResult<List<PerformanceReview>>> GetEmployeeReviews(int employeeId) => 
        Ok(await _service.GetEmployeeReviewsAsync(employeeId));

    [HttpPost("reviews")]
    public async Task<ActionResult<PerformanceReview>> CreateReview(PerformanceReview review) => 
        Ok(await _service.CreateReviewAsync(review));

    [HttpPut("reviews/{id}")]
    public async Task<ActionResult<PerformanceReview>> UpdateReview(int id, PerformanceReview review)
    {
        var updated = await _service.UpdateReviewAsync(id, review);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("feedback")]
    public async Task<ActionResult<List<Feedback360>>> GetAllFeedback() => 
        Ok(await _service.GetAllFeedbackAsync());

    [HttpGet("feedback/employee/{employeeId}")]
    public async Task<ActionResult<List<Feedback360>>> GetEmployeeFeedback(int employeeId) => 
        Ok(await _service.GetEmployeeFeedbackAsync(employeeId));

    [HttpPost("feedback")]
    public async Task<ActionResult<Feedback360>> SubmitFeedback(Feedback360 feedback) => 
        Ok(await _service.SubmitFeedbackAsync(feedback));

    [HttpGet("appraisals")]
    public async Task<ActionResult<List<Appraisal>>> GetAllAppraisals() => 
        Ok(await _service.GetAllAppraisalsAsync());

    [HttpGet("appraisals/employee/{employeeId}")]
    public async Task<ActionResult<List<Appraisal>>> GetEmployeeAppraisals(int employeeId) => 
        Ok(await _service.GetEmployeeAppraisalsAsync(employeeId));

    [HttpPost("appraisals")]
    public async Task<ActionResult<Appraisal>> CreateAppraisal(Appraisal appraisal) => 
        Ok(await _service.CreateAppraisalAsync(appraisal));

    [HttpPut("appraisals/{id}/approve")]
    public async Task<ActionResult<Appraisal>> ApproveAppraisal(int id, string approvedBy)
    {
        var updated = await _service.ApproveAppraisalAsync(id, approvedBy);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("stats")]
    public async Task<ActionResult<object>> GetStats() => Ok(await _service.GetPerformanceStatsAsync());
}
