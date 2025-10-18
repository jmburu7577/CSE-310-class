using EmployeeManagement.Web.Models;
using System.Text.Json;

namespace EmployeeManagement.Web.Services;

public class TrainingService : ITrainingService
{
    private readonly string _coursesFile = "Data/training_courses.json";
    private readonly string _schedulesFile = "Data/training_schedules.json";
    private readonly string _enrollmentsFile = "Data/training_enrollments.json";
    private readonly string _certificationsFile = "Data/certifications.json";
    private readonly string _skillsFile = "Data/employee_skills.json";
    private readonly string _skillGapFile = "Data/skill_gap_analysis.json";
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public TrainingService()
    {
        Directory.CreateDirectory("Data");
    }

    // Courses
    public async Task<List<TrainingCourse>> GetAllCoursesAsync()
    {
        return await ReadFromFileAsync<TrainingCourse>(_coursesFile);
    }

    public async Task<TrainingCourse?> GetCourseByIdAsync(int id)
    {
        var courses = await GetAllCoursesAsync();
        return courses.FirstOrDefault(c => c.Id == id);
    }

    public async Task<TrainingCourse> CreateCourseAsync(TrainingCourse course)
    {
        var courses = await GetAllCoursesAsync();
        course.Id = courses.Any() ? courses.Max(c => c.Id) + 1 : 1;
        courses.Add(course);
        await SaveToFileAsync(_coursesFile, courses);
        return course;
    }

    public async Task<TrainingCourse?> UpdateCourseAsync(int id, TrainingCourse course)
    {
        var courses = await GetAllCoursesAsync();
        var existing = courses.FirstOrDefault(c => c.Id == id);
        if (existing == null) return null;

        var index = courses.IndexOf(existing);
        course.Id = id;
        courses[index] = course;
        await SaveToFileAsync(_coursesFile, courses);
        return course;
    }

    public async Task<bool> DeleteCourseAsync(int id)
    {
        var courses = await GetAllCoursesAsync();
        var course = courses.FirstOrDefault(c => c.Id == id);
        if (course == null) return false;

        courses.Remove(course);
        await SaveToFileAsync(_coursesFile, courses);
        return true;
    }

    // Schedules
    public async Task<List<TrainingSchedule>> GetAllSchedulesAsync()
    {
        return await ReadFromFileAsync<TrainingSchedule>(_schedulesFile);
    }

    public async Task<TrainingSchedule?> GetScheduleByIdAsync(int id)
    {
        var schedules = await GetAllSchedulesAsync();
        return schedules.FirstOrDefault(s => s.Id == id);
    }

    public async Task<TrainingSchedule> CreateScheduleAsync(TrainingSchedule schedule)
    {
        var schedules = await GetAllSchedulesAsync();
        schedule.Id = schedules.Any() ? schedules.Max(s => s.Id) + 1 : 1;
        schedules.Add(schedule);
        await SaveToFileAsync(_schedulesFile, schedules);
        return schedule;
    }

    public async Task<TrainingSchedule?> UpdateScheduleAsync(int id, TrainingSchedule schedule)
    {
        var schedules = await GetAllSchedulesAsync();
        var existing = schedules.FirstOrDefault(s => s.Id == id);
        if (existing == null) return null;

        var index = schedules.IndexOf(existing);
        schedule.Id = id;
        schedules[index] = schedule;
        await SaveToFileAsync(_schedulesFile, schedules);
        return schedule;
    }

    // Enrollments
    public async Task<List<TrainingEnrollment>> GetAllEnrollmentsAsync()
    {
        return await ReadFromFileAsync<TrainingEnrollment>(_enrollmentsFile);
    }

    public async Task<List<TrainingEnrollment>> GetEmployeeEnrollmentsAsync(int employeeId)
    {
        var enrollments = await GetAllEnrollmentsAsync();
        return enrollments.Where(e => e.EmployeeId == employeeId).ToList();
    }

    public async Task<TrainingEnrollment> EnrollEmployeeAsync(TrainingEnrollment enrollment)
    {
        var enrollments = await GetAllEnrollmentsAsync();
        enrollment.Id = enrollments.Any() ? enrollments.Max(e => e.Id) + 1 : 1;
        enrollment.EnrollmentDate = DateTime.UtcNow;
        enrollment.Status = EnrollmentStatus.Enrolled;
        enrollments.Add(enrollment);
        await SaveToFileAsync(_enrollmentsFile, enrollments);
        return enrollment;
    }

    public async Task<TrainingEnrollment?> CompleteTrainingAsync(int id, int score, string feedback)
    {
        var enrollments = await GetAllEnrollmentsAsync();
        var enrollment = enrollments.FirstOrDefault(e => e.Id == id);
        if (enrollment == null) return null;

        enrollment.Status = score >= 70 ? EnrollmentStatus.Passed : EnrollmentStatus.Failed;
        enrollment.CompletionDate = DateTime.UtcNow;
        enrollment.Score = score;
        enrollment.Feedback = feedback;
        enrollment.CertificateIssued = score >= 70;
        
        await SaveToFileAsync(_enrollmentsFile, enrollments);
        return enrollment;
    }

    // Certifications
    public async Task<List<Certification>> GetAllCertificationsAsync()
    {
        return await ReadFromFileAsync<Certification>(_certificationsFile);
    }

    public async Task<List<Certification>> GetEmployeeCertificationsAsync(int employeeId)
    {
        var certifications = await GetAllCertificationsAsync();
        return certifications.Where(c => c.EmployeeId == employeeId).ToList();
    }

    public async Task<Certification> AddCertificationAsync(Certification certification)
    {
        var certifications = await GetAllCertificationsAsync();
        certification.Id = certifications.Any() ? certifications.Max(c => c.Id) + 1 : 1;
        certifications.Add(certification);
        await SaveToFileAsync(_certificationsFile, certifications);
        return certification;
    }

    public async Task<bool> DeleteCertificationAsync(int id)
    {
        var certifications = await GetAllCertificationsAsync();
        var certification = certifications.FirstOrDefault(c => c.Id == id);
        if (certification == null) return false;

        certifications.Remove(certification);
        await SaveToFileAsync(_certificationsFile, certifications);
        return true;
    }

    // Skills
    public async Task<List<EmployeeSkill>> GetEmployeeSkillsAsync(int employeeId)
    {
        var allSkills = await ReadFromFileAsync<EmployeeSkill>(_skillsFile);
        return allSkills.Where(s => s.EmployeeId == employeeId).ToList();
    }

    public async Task<EmployeeSkill> AddSkillAsync(EmployeeSkill skill)
    {
        var skills = await ReadFromFileAsync<EmployeeSkill>(_skillsFile);
        skill.Id = skills.Any() ? skills.Max(s => s.Id) + 1 : 1;
        skills.Add(skill);
        await SaveToFileAsync(_skillsFile, skills);
        return skill;
    }

    public async Task<EmployeeSkill?> UpdateSkillAsync(int id, EmployeeSkill skill)
    {
        var skills = await ReadFromFileAsync<EmployeeSkill>(_skillsFile);
        var existing = skills.FirstOrDefault(s => s.Id == id);
        if (existing == null) return null;

        var index = skills.IndexOf(existing);
        skill.Id = id;
        skills[index] = skill;
        await SaveToFileAsync(_skillsFile, skills);
        return skill;
    }

    public async Task<bool> DeleteSkillAsync(int id)
    {
        var skills = await ReadFromFileAsync<EmployeeSkill>(_skillsFile);
        var skill = skills.FirstOrDefault(s => s.Id == id);
        if (skill == null) return false;

        skills.Remove(skill);
        await SaveToFileAsync(_skillsFile, skills);
        return true;
    }

    // Skill Gap Analysis
    public async Task<SkillGapAnalysis?> GetSkillGapAnalysisAsync(int employeeId)
    {
        var analyses = await ReadFromFileAsync<SkillGapAnalysis>(_skillGapFile);
        return analyses.OrderByDescending(a => a.AnalysisDate)
            .FirstOrDefault(a => a.EmployeeId == employeeId);
    }

    public async Task<SkillGapAnalysis> CreateSkillGapAnalysisAsync(SkillGapAnalysis analysis)
    {
        var analyses = await ReadFromFileAsync<SkillGapAnalysis>(_skillGapFile);
        analysis.Id = analyses.Any() ? analyses.Max(a => a.Id) + 1 : 1;
        analysis.AnalysisDate = DateTime.UtcNow;
        analyses.Add(analysis);
        await SaveToFileAsync(_skillGapFile, analyses);
        return analysis;
    }

    // Statistics
    public async Task<object> GetTrainingStatsAsync()
    {
        var courses = await GetAllCoursesAsync();
        var enrollments = await GetAllEnrollmentsAsync();
        var certifications = await GetAllCertificationsAsync();

        return new
        {
            TotalCourses = courses.Count,
            ActiveCourses = courses.Count(c => c.Status == CourseStatus.Active),
            TotalEnrollments = enrollments.Count,
            CompletedTrainings = enrollments.Count(e => e.Status == EnrollmentStatus.Completed || e.Status == EnrollmentStatus.Passed),
            CertificatesIssued = enrollments.Count(e => e.CertificateIssued),
            ActiveCertifications = certifications.Count(c => c.Status == CertificationStatus.Active)
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
