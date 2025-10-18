using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Services;

namespace EmployeeManagement.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OffboardingController : ControllerBase
{
    private readonly IOffboardingService _service;

    public OffboardingController(IOffboardingService service) => _service = service;

    [HttpGet("resignations")]
    public async Task<ActionResult<List<Resignation>>> GetAllResignations() => 
        Ok(await _service.GetAllResignationsAsync());

    [HttpGet("resignations/{id}")]
    public async Task<ActionResult<Resignation>> GetResignation(int id)
    {
        var resignation = await _service.GetResignationByIdAsync(id);
        return resignation == null ? NotFound() : Ok(resignation);
    }

    [HttpPost("resignations")]
    public async Task<ActionResult<Resignation>> SubmitResignation(Resignation resignation) => 
        Ok(await _service.SubmitResignationAsync(resignation));

    [HttpPut("resignations/{id}/status")]
    public async Task<ActionResult<Resignation>> UpdateResignationStatus(int id, ResignationStatus status)
    {
        var updated = await _service.UpdateResignationStatusAsync(id, status);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("terminations")]
    public async Task<ActionResult<List<Termination>>> GetAllTerminations() => 
        Ok(await _service.GetAllTerminationsAsync());

    [HttpPost("terminations")]
    public async Task<ActionResult<Termination>> CreateTermination(Termination termination) => 
        Ok(await _service.CreateTerminationAsync(termination));

    [HttpGet("exit-interviews")]
    public async Task<ActionResult<List<ExitInterview>>> GetAllExitInterviews() => 
        Ok(await _service.GetAllExitInterviewsAsync());

    [HttpGet("exit-interviews/{id}")]
    public async Task<ActionResult<ExitInterview>> GetExitInterview(int id)
    {
        var interview = await _service.GetExitInterviewByIdAsync(id);
        return interview == null ? NotFound() : Ok(interview);
    }

    [HttpPost("exit-interviews")]
    public async Task<ActionResult<ExitInterview>> CreateExitInterview(ExitInterview interview) => 
        Ok(await _service.CreateExitInterviewAsync(interview));

    [HttpPut("exit-interviews/{id}")]
    public async Task<ActionResult<ExitInterview>> UpdateExitInterview(int id, ExitInterview interview)
    {
        var updated = await _service.UpdateExitInterviewAsync(id, interview);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("clearances")]
    public async Task<ActionResult<List<ExitClearance>>> GetAllClearances() => 
        Ok(await _service.GetAllClearancesAsync());

    [HttpGet("clearances/employee/{employeeId}")]
    public async Task<ActionResult<ExitClearance>> GetEmployeeClearance(int employeeId)
    {
        var clearance = await _service.GetClearanceByEmployeeIdAsync(employeeId);
        return clearance == null ? NotFound() : Ok(clearance);
    }

    [HttpPost("clearances")]
    public async Task<ActionResult<ExitClearance>> InitiateClearance(ExitClearance clearance) => 
        Ok(await _service.InitiateClearanceAsync(clearance));

    [HttpPut("clearances/{clearanceId}/items/{itemIndex}")]
    public async Task<ActionResult<ExitClearance>> UpdateClearanceItem(int clearanceId, 
        int itemIndex, bool isCleared, string clearedBy)
    {
        var updated = await _service.UpdateClearanceItemAsync(clearanceId, itemIndex, isCleared, clearedBy);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("settlements")]
    public async Task<ActionResult<List<FinalSettlement>>> GetAllSettlements() => 
        Ok(await _service.GetAllSettlementsAsync());

    [HttpGet("settlements/employee/{employeeId}")]
    public async Task<ActionResult<FinalSettlement>> GetEmployeeSettlement(int employeeId)
    {
        var settlement = await _service.GetSettlementByEmployeeIdAsync(employeeId);
        return settlement == null ? NotFound() : Ok(settlement);
    }

    [HttpPost("settlements")]
    public async Task<ActionResult<FinalSettlement>> CalculateSettlement(FinalSettlement settlement) => 
        Ok(await _service.CalculateSettlementAsync(settlement));

    [HttpPut("settlements/{id}/pay")]
    public async Task<ActionResult<FinalSettlement>> ProcessPayment(int id, string paymentMode)
    {
        var updated = await _service.ProcessPaymentAsync(id, paymentMode);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("stats")]
    public async Task<ActionResult<object>> GetStats() => Ok(await _service.GetOffboardingStatsAsync());
}
