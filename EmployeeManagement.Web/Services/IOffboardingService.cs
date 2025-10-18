using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

public interface IOffboardingService
{
    // Resignations
    Task<List<Resignation>> GetAllResignationsAsync();
    Task<Resignation?> GetResignationByIdAsync(int id);
    Task<Resignation> SubmitResignationAsync(Resignation resignation);
    Task<Resignation?> UpdateResignationStatusAsync(int id, ResignationStatus status);
    
    // Terminations
    Task<List<Termination>> GetAllTerminationsAsync();
    Task<Termination?> GetTerminationByIdAsync(int id);
    Task<Termination> CreateTerminationAsync(Termination termination);
    
    // Exit Interviews
    Task<List<ExitInterview>> GetAllExitInterviewsAsync();
    Task<ExitInterview?> GetExitInterviewByIdAsync(int id);
    Task<ExitInterview> CreateExitInterviewAsync(ExitInterview interview);
    Task<ExitInterview?> UpdateExitInterviewAsync(int id, ExitInterview interview);
    
    // Exit Clearance
    Task<List<ExitClearance>> GetAllClearancesAsync();
    Task<ExitClearance?> GetClearanceByEmployeeIdAsync(int employeeId);
    Task<ExitClearance> InitiateClearanceAsync(ExitClearance clearance);
    Task<ExitClearance?> UpdateClearanceItemAsync(int clearanceId, int itemIndex, bool isCleared, string clearedBy);
    
    // Final Settlement
    Task<List<FinalSettlement>> GetAllSettlementsAsync();
    Task<FinalSettlement?> GetSettlementByEmployeeIdAsync(int employeeId);
    Task<FinalSettlement> CalculateSettlementAsync(FinalSettlement settlement);
    Task<FinalSettlement?> ProcessPaymentAsync(int id, string paymentMode);
    
    // Statistics
    Task<object> GetOffboardingStatsAsync();
}
