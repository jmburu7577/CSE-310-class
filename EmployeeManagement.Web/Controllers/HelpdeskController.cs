using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Services;

namespace EmployeeManagement.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HelpdeskController : ControllerBase
{
    private readonly IHelpdeskService _service;

    public HelpdeskController(IHelpdeskService service) => _service = service;

    [HttpGet("tickets")]
    public async Task<ActionResult<List<HRTicket>>> GetAllTickets() => 
        Ok(await _service.GetAllTicketsAsync());

    [HttpGet("tickets/{id}")]
    public async Task<ActionResult<HRTicket>> GetTicket(int id)
    {
        var ticket = await _service.GetTicketByIdAsync(id);
        return ticket == null ? NotFound() : Ok(ticket);
    }

    [HttpGet("tickets/employee/{employeeId}")]
    public async Task<ActionResult<List<HRTicket>>> GetEmployeeTickets(int employeeId) => 
        Ok(await _service.GetEmployeeTicketsAsync(employeeId));

    [HttpPost("tickets")]
    public async Task<ActionResult<HRTicket>> CreateTicket(HRTicket ticket) => 
        Ok(await _service.CreateTicketAsync(ticket));

    [HttpPut("tickets/{id}/status")]
    public async Task<ActionResult<HRTicket>> UpdateTicketStatus(int id, TicketStatus status)
    {
        var updated = await _service.UpdateTicketStatusAsync(id, status);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpPut("tickets/{id}/assign")]
    public async Task<ActionResult<HRTicket>> AssignTicket(int id, string assignedTo)
    {
        var updated = await _service.AssignTicketAsync(id, assignedTo);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpPost("tickets/{id}/comments")]
    public async Task<ActionResult<HRTicket>> AddComment(int id, TicketComment comment)
    {
        var updated = await _service.AddCommentAsync(id, comment);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpPut("tickets/{id}/resolve")]
    public async Task<ActionResult<HRTicket>> ResolveTicket(int id)
    {
        var updated = await _service.ResolveTicketAsync(id);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("knowledge-base")]
    public async Task<ActionResult<List<KnowledgeBaseArticle>>> GetAllArticles() => 
        Ok(await _service.GetAllArticlesAsync());

    [HttpGet("knowledge-base/search")]
    public async Task<ActionResult<List<KnowledgeBaseArticle>>> SearchArticles(string term) => 
        Ok(await _service.SearchArticlesAsync(term));

    [HttpPost("knowledge-base")]
    public async Task<ActionResult<KnowledgeBaseArticle>> CreateArticle(KnowledgeBaseArticle article) => 
        Ok(await _service.CreateArticleAsync(article));

    [HttpPut("knowledge-base/{id}")]
    public async Task<ActionResult<KnowledgeBaseArticle>> UpdateArticle(int id, KnowledgeBaseArticle article)
    {
        var updated = await _service.UpdateArticleAsync(id, article);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpGet("stats")]
    public async Task<ActionResult<object>> GetStats() => Ok(await _service.GetHelpdeskStatsAsync());
}
