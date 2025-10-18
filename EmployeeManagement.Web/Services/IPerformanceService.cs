using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

public interface IPerformanceService
{
    // Performance Goals
    Task<List<PerformanceGoal>> GetAllGoalsAsync();
    Task<PerformanceGoal?> GetGoalByIdAsync(int id);
    Task<List<PerformanceGoal>> GetEmployeeGoalsAsync(int employeeId);
    Task<PerformanceGoal> CreateGoalAsync(PerformanceGoal goal);
    Task<PerformanceGoal?> UpdateGoalAsync(int id, PerformanceGoal goal);
    Task<bool> DeleteGoalAsync(int id);
    
    // Performance Reviews
    Task<List<PerformanceReview>> GetAllReviewsAsync();
    Task<PerformanceReview?> GetReviewByIdAsync(int id);
    Task<List<PerformanceReview>> GetEmployeeReviewsAsync(int employeeId);
    Task<PerformanceReview> CreateReviewAsync(PerformanceReview review);
    Task<PerformanceReview?> UpdateReviewAsync(int id, PerformanceReview review);
    
    // 360 Feedback
    Task<List<Feedback360>> GetAllFeedbackAsync();
    Task<List<Feedback360>> GetEmployeeFeedbackAsync(int employeeId);
    Task<Feedback360> SubmitFeedbackAsync(Feedback360 feedback);
    
    // Appraisals
    Task<List<Appraisal>> GetAllAppraisalsAsync();
    Task<Appraisal?> GetAppraisalByIdAsync(int id);
    Task<List<Appraisal>> GetEmployeeAppraisalsAsync(int employeeId);
    Task<Appraisal> CreateAppraisalAsync(Appraisal appraisal);
    Task<Appraisal?> ApproveAppraisalAsync(int id, string approvedBy);
    
    // Statistics
    Task<object> GetPerformanceStatsAsync();
}
