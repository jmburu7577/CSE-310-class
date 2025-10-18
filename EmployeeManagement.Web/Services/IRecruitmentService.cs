using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

public interface IRecruitmentService
{
    // Job Postings
    Task<List<JobPosting>> GetAllJobPostingsAsync();
    Task<JobPosting?> GetJobPostingByIdAsync(int id);
    Task<JobPosting> CreateJobPostingAsync(JobPosting jobPosting);
    Task<JobPosting?> UpdateJobPostingAsync(int id, JobPosting jobPosting);
    Task<bool> DeleteJobPostingAsync(int id);
    Task<List<JobPosting>> GetActiveJobPostingsAsync();
    
    // Applications
    Task<List<JobApplication>> GetAllApplicationsAsync();
    Task<JobApplication?> GetApplicationByIdAsync(int id);
    Task<List<JobApplication>> GetApplicationsByJobIdAsync(int jobId);
    Task<JobApplication> CreateApplicationAsync(JobApplication application);
    Task<JobApplication?> UpdateApplicationStatusAsync(int id, ApplicationStatus status, string notes);
    Task<bool> DeleteApplicationAsync(int id);
    
    // Interviews
    Task<List<Interview>> GetAllInterviewsAsync();
    Task<Interview?> GetInterviewByIdAsync(int id);
    Task<List<Interview>> GetInterviewsByApplicationIdAsync(int applicationId);
    Task<Interview> ScheduleInterviewAsync(Interview interview);
    Task<Interview?> UpdateInterviewAsync(int id, Interview interview);
    Task<bool> CancelInterviewAsync(int id);
    
    // Offer Letters
    Task<List<OfferLetter>> GetAllOfferLettersAsync();
    Task<OfferLetter?> GetOfferLetterByIdAsync(int id);
    Task<OfferLetter> CreateOfferLetterAsync(OfferLetter offerLetter);
    Task<OfferLetter?> UpdateOfferStatusAsync(int id, OfferStatus status);
    
    // Onboarding
    Task<List<OnboardingTask>> GetOnboardingTasksByEmployeeIdAsync(int employeeId);
    Task<OnboardingTask> CreateOnboardingTaskAsync(OnboardingTask task);
    Task<OnboardingTask?> CompleteOnboardingTaskAsync(int id, string completedBy);
    
    // Statistics
    Task<object> GetRecruitmentStatsAsync();
}
