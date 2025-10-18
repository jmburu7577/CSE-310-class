using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

public interface ITrainingService
{
    // Training Courses
    Task<List<TrainingCourse>> GetAllCoursesAsync();
    Task<TrainingCourse?> GetCourseByIdAsync(int id);
    Task<TrainingCourse> CreateCourseAsync(TrainingCourse course);
    Task<TrainingCourse?> UpdateCourseAsync(int id, TrainingCourse course);
    Task<bool> DeleteCourseAsync(int id);
    
    // Training Schedules
    Task<List<TrainingSchedule>> GetAllSchedulesAsync();
    Task<TrainingSchedule?> GetScheduleByIdAsync(int id);
    Task<TrainingSchedule> CreateScheduleAsync(TrainingSchedule schedule);
    Task<TrainingSchedule?> UpdateScheduleAsync(int id, TrainingSchedule schedule);
    
    // Enrollments
    Task<List<TrainingEnrollment>> GetAllEnrollmentsAsync();
    Task<List<TrainingEnrollment>> GetEmployeeEnrollmentsAsync(int employeeId);
    Task<TrainingEnrollment> EnrollEmployeeAsync(TrainingEnrollment enrollment);
    Task<TrainingEnrollment?> CompleteTrainingAsync(int id, int score, string feedback);
    
    // Certifications
    Task<List<Certification>> GetAllCertificationsAsync();
    Task<List<Certification>> GetEmployeeCertificationsAsync(int employeeId);
    Task<Certification> AddCertificationAsync(Certification certification);
    Task<bool> DeleteCertificationAsync(int id);
    
    // Skills
    Task<List<EmployeeSkill>> GetEmployeeSkillsAsync(int employeeId);
    Task<EmployeeSkill> AddSkillAsync(EmployeeSkill skill);
    Task<EmployeeSkill?> UpdateSkillAsync(int id, EmployeeSkill skill);
    Task<bool> DeleteSkillAsync(int id);
    
    // Skill Gap Analysis
    Task<SkillGapAnalysis?> GetSkillGapAnalysisAsync(int employeeId);
    Task<SkillGapAnalysis> CreateSkillGapAnalysisAsync(SkillGapAnalysis analysis);
    
    // Statistics
    Task<object> GetTrainingStatsAsync();
}
