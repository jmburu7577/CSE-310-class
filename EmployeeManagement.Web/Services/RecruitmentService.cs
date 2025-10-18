using EmployeeManagement.Web.Models;
using System.Text.Json;

namespace EmployeeManagement.Web.Services;

public class RecruitmentService : IRecruitmentService
{
    private readonly string _jobPostingsFile = "Data/job_postings.json";
    private readonly string _applicationsFile = "Data/applications.json";
    private readonly string _interviewsFile = "Data/interviews.json";
    private readonly string _offersFile = "Data/offers.json";
    private readonly string _onboardingFile = "Data/onboarding_tasks.json";
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public RecruitmentService()
    {
        Directory.CreateDirectory("Data");
    }

    // Job Postings
    public async Task<List<JobPosting>> GetAllJobPostingsAsync()
    {
        return await ReadFromFileAsync<JobPosting>(_jobPostingsFile);
    }

    public async Task<JobPosting?> GetJobPostingByIdAsync(int id)
    {
        var postings = await GetAllJobPostingsAsync();
        return postings.FirstOrDefault(j => j.Id == id);
    }

    public async Task<JobPosting> CreateJobPostingAsync(JobPosting jobPosting)
    {
        var postings = await GetAllJobPostingsAsync();
        jobPosting.Id = postings.Any() ? postings.Max(j => j.Id) + 1 : 1;
        jobPosting.PostedDate = DateTime.UtcNow;
        postings.Add(jobPosting);
        await SaveToFileAsync(_jobPostingsFile, postings);
        return jobPosting;
    }

    public async Task<JobPosting?> UpdateJobPostingAsync(int id, JobPosting jobPosting)
    {
        var postings = await GetAllJobPostingsAsync();
        var existing = postings.FirstOrDefault(j => j.Id == id);
        if (existing == null) return null;

        var index = postings.IndexOf(existing);
        jobPosting.Id = id;
        postings[index] = jobPosting;
        await SaveToFileAsync(_jobPostingsFile, postings);
        return jobPosting;
    }

    public async Task<bool> DeleteJobPostingAsync(int id)
    {
        var postings = await GetAllJobPostingsAsync();
        var posting = postings.FirstOrDefault(j => j.Id == id);
        if (posting == null) return false;

        postings.Remove(posting);
        await SaveToFileAsync(_jobPostingsFile, postings);
        return true;
    }

    public async Task<List<JobPosting>> GetActiveJobPostingsAsync()
    {
        var postings = await GetAllJobPostingsAsync();
        return postings.Where(j => j.Status == JobStatus.Open).ToList();
    }

    // Applications
    public async Task<List<JobApplication>> GetAllApplicationsAsync()
    {
        return await ReadFromFileAsync<JobApplication>(_applicationsFile);
    }

    public async Task<JobApplication?> GetApplicationByIdAsync(int id)
    {
        var applications = await GetAllApplicationsAsync();
        return applications.FirstOrDefault(a => a.Id == id);
    }

    public async Task<List<JobApplication>> GetApplicationsByJobIdAsync(int jobId)
    {
        var applications = await GetAllApplicationsAsync();
        return applications.Where(a => a.JobPostingId == jobId).ToList();
    }

    public async Task<JobApplication> CreateApplicationAsync(JobApplication application)
    {
        var applications = await GetAllApplicationsAsync();
        application.Id = applications.Any() ? applications.Max(a => a.Id) + 1 : 1;
        application.AppliedDate = DateTime.UtcNow;
        application.Status = ApplicationStatus.Submitted;
        applications.Add(application);
        await SaveToFileAsync(_applicationsFile, applications);
        return application;
    }

    public async Task<JobApplication?> UpdateApplicationStatusAsync(int id, ApplicationStatus status, string notes)
    {
        var applications = await GetAllApplicationsAsync();
        var application = applications.FirstOrDefault(a => a.Id == id);
        if (application == null) return null;

        application.Status = status;
        application.Notes = notes;
        await SaveToFileAsync(_applicationsFile, applications);
        return application;
    }

    public async Task<bool> DeleteApplicationAsync(int id)
    {
        var applications = await GetAllApplicationsAsync();
        var application = applications.FirstOrDefault(a => a.Id == id);
        if (application == null) return false;

        applications.Remove(application);
        await SaveToFileAsync(_applicationsFile, applications);
        return true;
    }

    // Interviews
    public async Task<List<Interview>> GetAllInterviewsAsync()
    {
        return await ReadFromFileAsync<Interview>(_interviewsFile);
    }

    public async Task<Interview?> GetInterviewByIdAsync(int id)
    {
        var interviews = await GetAllInterviewsAsync();
        return interviews.FirstOrDefault(i => i.Id == id);
    }

    public async Task<List<Interview>> GetInterviewsByApplicationIdAsync(int applicationId)
    {
        var interviews = await GetAllInterviewsAsync();
        return interviews.Where(i => i.ApplicationId == applicationId).ToList();
    }

    public async Task<Interview> ScheduleInterviewAsync(Interview interview)
    {
        var interviews = await GetAllInterviewsAsync();
        interview.Id = interviews.Any() ? interviews.Max(i => i.Id) + 1 : 1;
        interview.Status = InterviewStatus.Scheduled;
        interviews.Add(interview);
        await SaveToFileAsync(_interviewsFile, interviews);
        return interview;
    }

    public async Task<Interview?> UpdateInterviewAsync(int id, Interview interview)
    {
        var interviews = await GetAllInterviewsAsync();
        var existing = interviews.FirstOrDefault(i => i.Id == id);
        if (existing == null) return null;

        var index = interviews.IndexOf(existing);
        interview.Id = id;
        interviews[index] = interview;
        await SaveToFileAsync(_interviewsFile, interviews);
        return interview;
    }

    public async Task<bool> CancelInterviewAsync(int id)
    {
        var interviews = await GetAllInterviewsAsync();
        var interview = interviews.FirstOrDefault(i => i.Id == id);
        if (interview == null) return false;

        interview.Status = InterviewStatus.Cancelled;
        await SaveToFileAsync(_interviewsFile, interviews);
        return true;
    }

    // Offer Letters
    public async Task<List<OfferLetter>> GetAllOfferLettersAsync()
    {
        return await ReadFromFileAsync<OfferLetter>(_offersFile);
    }

    public async Task<OfferLetter?> GetOfferLetterByIdAsync(int id)
    {
        var offers = await GetAllOfferLettersAsync();
        return offers.FirstOrDefault(o => o.Id == id);
    }

    public async Task<OfferLetter> CreateOfferLetterAsync(OfferLetter offerLetter)
    {
        var offers = await GetAllOfferLettersAsync();
        offerLetter.Id = offers.Any() ? offers.Max(o => o.Id) + 1 : 1;
        offerLetter.OfferDate = DateTime.UtcNow;
        offerLetter.Status = OfferStatus.Draft;
        offers.Add(offerLetter);
        await SaveToFileAsync(_offersFile, offers);
        return offerLetter;
    }

    public async Task<OfferLetter?> UpdateOfferStatusAsync(int id, OfferStatus status)
    {
        var offers = await GetAllOfferLettersAsync();
        var offer = offers.FirstOrDefault(o => o.Id == id);
        if (offer == null) return null;

        offer.Status = status;
        await SaveToFileAsync(_offersFile, offers);
        return offer;
    }

    // Onboarding
    public async Task<List<OnboardingTask>> GetOnboardingTasksByEmployeeIdAsync(int employeeId)
    {
        var tasks = await ReadFromFileAsync<OnboardingTask>(_onboardingFile);
        return tasks.Where(t => t.EmployeeId == employeeId).ToList();
    }

    public async Task<OnboardingTask> CreateOnboardingTaskAsync(OnboardingTask task)
    {
        var tasks = await ReadFromFileAsync<OnboardingTask>(_onboardingFile);
        task.Id = tasks.Any() ? tasks.Max(t => t.Id) + 1 : 1;
        tasks.Add(task);
        await SaveToFileAsync(_onboardingFile, tasks);
        return task;
    }

    public async Task<OnboardingTask?> CompleteOnboardingTaskAsync(int id, string completedBy)
    {
        var tasks = await ReadFromFileAsync<OnboardingTask>(_onboardingFile);
        var task = tasks.FirstOrDefault(t => t.Id == id);
        if (task == null) return null;

        task.IsCompleted = true;
        task.CompletedDate = DateTime.UtcNow;
        task.CompletedBy = completedBy;
        await SaveToFileAsync(_onboardingFile, tasks);
        return task;
    }

    // Statistics
    public async Task<object> GetRecruitmentStatsAsync()
    {
        var postings = await GetAllJobPostingsAsync();
        var applications = await GetAllApplicationsAsync();
        var interviews = await GetAllInterviewsAsync();
        var offers = await GetAllOfferLettersAsync();

        return new
        {
            TotalJobPostings = postings.Count,
            ActiveJobPostings = postings.Count(j => j.Status == JobStatus.Open),
            TotalApplications = applications.Count,
            ApplicationsByStatus = applications.GroupBy(a => a.Status)
                .ToDictionary(g => g.Key.ToString(), g => g.Count()),
            InterviewsScheduled = interviews.Count(i => i.Status == InterviewStatus.Scheduled),
            OffersExtended = offers.Count(o => o.Status == OfferStatus.Sent),
            OffersAccepted = offers.Count(o => o.Status == OfferStatus.Accepted)
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
