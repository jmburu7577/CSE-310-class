using EmployeeManagement.Web.Models;
using System.Text.Json;

namespace EmployeeManagement.Web.Services;

public class PerformanceService : IPerformanceService
{
    private readonly string _goalsFile = "Data/performance_goals.json";
    private readonly string _reviewsFile = "Data/performance_reviews.json";
    private readonly string _feedbackFile = "Data/feedback_360.json";
    private readonly string _appraisalsFile = "Data/appraisals.json";
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public PerformanceService()
    {
        Directory.CreateDirectory("Data");
    }

    // Goals
    public async Task<List<PerformanceGoal>> GetAllGoalsAsync()
    {
        return await ReadFromFileAsync<PerformanceGoal>(_goalsFile);
    }

    public async Task<PerformanceGoal?> GetGoalByIdAsync(int id)
    {
        var goals = await GetAllGoalsAsync();
        return goals.FirstOrDefault(g => g.Id == id);
    }

    public async Task<List<PerformanceGoal>> GetEmployeeGoalsAsync(int employeeId)
    {
        var goals = await GetAllGoalsAsync();
        return goals.Where(g => g.EmployeeId == employeeId).ToList();
    }

    public async Task<PerformanceGoal> CreateGoalAsync(PerformanceGoal goal)
    {
        var goals = await GetAllGoalsAsync();
        goal.Id = goals.Any() ? goals.Max(g => g.Id) + 1 : 1;
        goal.Status = GoalStatus.NotStarted;
        goals.Add(goal);
        await SaveToFileAsync(_goalsFile, goals);
        return goal;
    }

    public async Task<PerformanceGoal?> UpdateGoalAsync(int id, PerformanceGoal goal)
    {
        var goals = await GetAllGoalsAsync();
        var existing = goals.FirstOrDefault(g => g.Id == id);
        if (existing == null) return null;

        var index = goals.IndexOf(existing);
        goal.Id = id;
        goals[index] = goal;
        await SaveToFileAsync(_goalsFile, goals);
        return goal;
    }

    public async Task<bool> DeleteGoalAsync(int id)
    {
        var goals = await GetAllGoalsAsync();
        var goal = goals.FirstOrDefault(g => g.Id == id);
        if (goal == null) return false;

        goals.Remove(goal);
        await SaveToFileAsync(_goalsFile, goals);
        return true;
    }

    // Reviews
    public async Task<List<PerformanceReview>> GetAllReviewsAsync()
    {
        return await ReadFromFileAsync<PerformanceReview>(_reviewsFile);
    }

    public async Task<PerformanceReview?> GetReviewByIdAsync(int id)
    {
        var reviews = await GetAllReviewsAsync();
        return reviews.FirstOrDefault(r => r.Id == id);
    }

    public async Task<List<PerformanceReview>> GetEmployeeReviewsAsync(int employeeId)
    {
        var reviews = await GetAllReviewsAsync();
        return reviews.Where(r => r.EmployeeId == employeeId).OrderByDescending(r => r.ReviewDate).ToList();
    }

    public async Task<PerformanceReview> CreateReviewAsync(PerformanceReview review)
    {
        var reviews = await GetAllReviewsAsync();
        review.Id = reviews.Any() ? reviews.Max(r => r.Id) + 1 : 1;
        review.Status = ReviewStatus.Pending;
        reviews.Add(review);
        await SaveToFileAsync(_reviewsFile, reviews);
        return review;
    }

    public async Task<PerformanceReview?> UpdateReviewAsync(int id, PerformanceReview review)
    {
        var reviews = await GetAllReviewsAsync();
        var existing = reviews.FirstOrDefault(r => r.Id == id);
        if (existing == null) return null;

        var index = reviews.IndexOf(existing);
        review.Id = id;
        reviews[index] = review;
        await SaveToFileAsync(_reviewsFile, reviews);
        return review;
    }

    // 360 Feedback
    public async Task<List<Feedback360>> GetAllFeedbackAsync()
    {
        return await ReadFromFileAsync<Feedback360>(_feedbackFile);
    }

    public async Task<List<Feedback360>> GetEmployeeFeedbackAsync(int employeeId)
    {
        var feedback = await GetAllFeedbackAsync();
        return feedback.Where(f => f.EmployeeId == employeeId).ToList();
    }

    public async Task<Feedback360> SubmitFeedbackAsync(Feedback360 feedback)
    {
        var allFeedback = await GetAllFeedbackAsync();
        feedback.Id = allFeedback.Any() ? allFeedback.Max(f => f.Id) + 1 : 1;
        feedback.SubmittedDate = DateTime.UtcNow;
        allFeedback.Add(feedback);
        await SaveToFileAsync(_feedbackFile, allFeedback);
        return feedback;
    }

    // Appraisals
    public async Task<List<Appraisal>> GetAllAppraisalsAsync()
    {
        return await ReadFromFileAsync<Appraisal>(_appraisalsFile);
    }

    public async Task<Appraisal?> GetAppraisalByIdAsync(int id)
    {
        var appraisals = await GetAllAppraisalsAsync();
        return appraisals.FirstOrDefault(a => a.Id == id);
    }

    public async Task<List<Appraisal>> GetEmployeeAppraisalsAsync(int employeeId)
    {
        var appraisals = await GetAllAppraisalsAsync();
        return appraisals.Where(a => a.EmployeeId == employeeId).OrderByDescending(a => a.AppraisalDate).ToList();
    }

    public async Task<Appraisal> CreateAppraisalAsync(Appraisal appraisal)
    {
        var appraisals = await GetAllAppraisalsAsync();
        appraisal.Id = appraisals.Any() ? appraisals.Max(a => a.Id) + 1 : 1;
        appraisal.Status = AppraisalStatus.Draft;
        appraisals.Add(appraisal);
        await SaveToFileAsync(_appraisalsFile, appraisals);
        return appraisal;
    }

    public async Task<Appraisal?> ApproveAppraisalAsync(int id, string approvedBy)
    {
        var appraisals = await GetAllAppraisalsAsync();
        var appraisal = appraisals.FirstOrDefault(a => a.Id == id);
        if (appraisal == null) return null;

        appraisal.Status = AppraisalStatus.Approved;
        appraisal.ApprovedBy = approvedBy;
        await SaveToFileAsync(_appraisalsFile, appraisals);
        return appraisal;
    }

    // Statistics
    public async Task<object> GetPerformanceStatsAsync()
    {
        var goals = await GetAllGoalsAsync();
        var reviews = await GetAllReviewsAsync();
        var appraisals = await GetAllAppraisalsAsync();

        return new
        {
            TotalGoals = goals.Count,
            GoalsByStatus = goals.GroupBy(g => g.Status).ToDictionary(g => g.Key.ToString(), g => g.Count()),
            CompletedGoals = goals.Count(g => g.Status == GoalStatus.Completed),
            TotalReviews = reviews.Count,
            AverageRating = reviews.Any() ? reviews.Average(r => r.OverallRating) : 0,
            PendingAppraisals = appraisals.Count(a => a.Status == AppraisalStatus.UnderReview)
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
