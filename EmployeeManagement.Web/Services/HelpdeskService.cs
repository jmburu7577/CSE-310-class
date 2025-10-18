using EmployeeManagement.Web.Models;
using System.Text.Json;

namespace EmployeeManagement.Web.Services;

public class HelpdeskService : IHelpdeskService
{
    private readonly string _ticketsFile = "Data/hr_tickets.json";
    private readonly string _articlesFile = "Data/knowledge_base.json";
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public HelpdeskService()
    {
        Directory.CreateDirectory("Data");
    }

    public async Task<List<HRTicket>> GetAllTicketsAsync()
    {
        return await ReadFromFileAsync<HRTicket>(_ticketsFile);
    }

    public async Task<HRTicket?> GetTicketByIdAsync(int id)
    {
        var tickets = await GetAllTicketsAsync();
        return tickets.FirstOrDefault(t => t.Id == id);
    }

    public async Task<List<HRTicket>> GetEmployeeTicketsAsync(int employeeId)
    {
        var tickets = await GetAllTicketsAsync();
        return tickets.Where(t => t.EmployeeId == employeeId).OrderByDescending(t => t.CreatedDate).ToList();
    }

    public async Task<HRTicket> CreateTicketAsync(HRTicket ticket)
    {
        var tickets = await GetAllTicketsAsync();
        ticket.Id = tickets.Any() ? tickets.Max(t => t.Id) + 1 : 1;
        ticket.CreatedDate = DateTime.UtcNow;
        ticket.Status = TicketStatus.Open;
        tickets.Add(ticket);
        await SaveToFileAsync(_ticketsFile, tickets);
        return ticket;
    }

    public async Task<HRTicket?> UpdateTicketStatusAsync(int id, TicketStatus status)
    {
        var tickets = await GetAllTicketsAsync();
        var ticket = tickets.FirstOrDefault(t => t.Id == id);
        if (ticket == null) return null;

        ticket.Status = status;
        if (status == TicketStatus.Resolved) ticket.ResolvedDate = DateTime.UtcNow;
        else if (status == TicketStatus.Closed) ticket.ClosedDate = DateTime.UtcNow;

        await SaveToFileAsync(_ticketsFile, tickets);
        return ticket;
    }

    public async Task<HRTicket?> AssignTicketAsync(int id, string assignedTo)
    {
        var tickets = await GetAllTicketsAsync();
        var ticket = tickets.FirstOrDefault(t => t.Id == id);
        if (ticket == null) return null;

        ticket.AssignedTo = assignedTo;
        ticket.Status = TicketStatus.InProgress;
        await SaveToFileAsync(_ticketsFile, tickets);
        return ticket;
    }

    public async Task<HRTicket?> AddCommentAsync(int id, TicketComment comment)
    {
        var tickets = await GetAllTicketsAsync();
        var ticket = tickets.FirstOrDefault(t => t.Id == id);
        if (ticket == null) return null;

        comment.Id = ticket.Comments.Any() ? ticket.Comments.Max(c => c.Id) + 1 : 1;
        comment.CreatedDate = DateTime.UtcNow;
        ticket.Comments.Add(comment);
        await SaveToFileAsync(_ticketsFile, tickets);
        return ticket;
    }

    public async Task<HRTicket?> ResolveTicketAsync(int id)
    {
        return await UpdateTicketStatusAsync(id, TicketStatus.Resolved);
    }

    public async Task<bool> DeleteTicketAsync(int id)
    {
        var tickets = await GetAllTicketsAsync();
        var ticket = tickets.FirstOrDefault(t => t.Id == id);
        if (ticket == null) return false;

        tickets.Remove(ticket);
        await SaveToFileAsync(_ticketsFile, tickets);
        return true;
    }

    public async Task<List<KnowledgeBaseArticle>> GetAllArticlesAsync()
    {
        return await ReadFromFileAsync<KnowledgeBaseArticle>(_articlesFile);
    }

    public async Task<KnowledgeBaseArticle?> GetArticleByIdAsync(int id)
    {
        var articles = await GetAllArticlesAsync();
        return articles.FirstOrDefault(a => a.Id == id);
    }

    public async Task<List<KnowledgeBaseArticle>> SearchArticlesAsync(string searchTerm)
    {
        var articles = await GetAllArticlesAsync();
        return articles.Where(a => a.IsPublished && 
            (a.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
             a.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
             a.Tags.Any(t => t.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))).ToList();
    }

    public async Task<KnowledgeBaseArticle> CreateArticleAsync(KnowledgeBaseArticle article)
    {
        var articles = await GetAllArticlesAsync();
        article.Id = articles.Any() ? articles.Max(a => a.Id) + 1 : 1;
        article.CreatedDate = DateTime.UtcNow;
        articles.Add(article);
        await SaveToFileAsync(_articlesFile, articles);
        return article;
    }

    public async Task<KnowledgeBaseArticle?> UpdateArticleAsync(int id, KnowledgeBaseArticle article)
    {
        var articles = await GetAllArticlesAsync();
        var existing = articles.FirstOrDefault(a => a.Id == id);
        if (existing == null) return null;

        var index = articles.IndexOf(existing);
        article.Id = id;
        article.UpdatedDate = DateTime.UtcNow;
        articles[index] = article;
        await SaveToFileAsync(_articlesFile, articles);
        return article;
    }

    public async Task<bool> DeleteArticleAsync(int id)
    {
        var articles = await GetAllArticlesAsync();
        var article = articles.FirstOrDefault(a => a.Id == id);
        if (article == null) return false;

        articles.Remove(article);
        await SaveToFileAsync(_articlesFile, articles);
        return true;
    }

    public async Task<object> GetHelpdeskStatsAsync()
    {
        var tickets = await GetAllTicketsAsync();

        return new
        {
            TotalTickets = tickets.Count,
            OpenTickets = tickets.Count(t => t.Status == TicketStatus.Open),
            InProgressTickets = tickets.Count(t => t.Status == TicketStatus.InProgress),
            ResolvedTickets = tickets.Count(t => t.Status == TicketStatus.Resolved),
            ClosedTickets = tickets.Count(t => t.Status == TicketStatus.Closed),
            TicketsByCategory = tickets.GroupBy(t => t.Category).ToDictionary(g => g.Key.ToString(), g => g.Count()),
            TicketsByPriority = tickets.GroupBy(t => t.Priority).ToDictionary(g => g.Key.ToString(), g => g.Count()),
            AverageResolutionTime = tickets.Where(t => t.ResolvedDate.HasValue)
                .Select(t => (t.ResolvedDate!.Value - t.CreatedDate).TotalHours).DefaultIfEmpty(0).Average()
        };
    }

    private async Task<List<T>> ReadFromFileAsync<T>(string filePath)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (!File.Exists(filePath)) return new List<T>();
            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
        finally { _semaphore.Release(); }
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
        finally { _semaphore.Release(); }
    }
}
