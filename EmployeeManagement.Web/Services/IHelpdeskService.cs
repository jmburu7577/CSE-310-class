using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

public interface IHelpdeskService
{
    // Tickets
    Task<List<HRTicket>> GetAllTicketsAsync();
    Task<HRTicket?> GetTicketByIdAsync(int id);
    Task<List<HRTicket>> GetEmployeeTicketsAsync(int employeeId);
    Task<HRTicket> CreateTicketAsync(HRTicket ticket);
    Task<HRTicket?> UpdateTicketStatusAsync(int id, TicketStatus status);
    Task<HRTicket?> AssignTicketAsync(int id, string assignedTo);
    Task<HRTicket?> AddCommentAsync(int id, TicketComment comment);
    Task<HRTicket?> ResolveTicketAsync(int id);
    Task<bool> DeleteTicketAsync(int id);
    
    // Knowledge Base
    Task<List<KnowledgeBaseArticle>> GetAllArticlesAsync();
    Task<KnowledgeBaseArticle?> GetArticleByIdAsync(int id);
    Task<List<KnowledgeBaseArticle>> SearchArticlesAsync(string searchTerm);
    Task<KnowledgeBaseArticle> CreateArticleAsync(KnowledgeBaseArticle article);
    Task<KnowledgeBaseArticle?> UpdateArticleAsync(int id, KnowledgeBaseArticle article);
    Task<bool> DeleteArticleAsync(int id);
    
    // Statistics
    Task<object> GetHelpdeskStatsAsync();
}
