using EmployeeManagement.Web.Models;
using System.Text.Json;

namespace EmployeeManagement.Web.Services;

public class OffboardingService : IOffboardingService
{
    private readonly string _resignationsFile = "Data/resignations.json";
    private readonly string _terminationsFile = "Data/terminations.json";
    private readonly string _exitInterviewsFile = "Data/exit_interviews.json";
    private readonly string _clearancesFile = "Data/exit_clearances.json";
    private readonly string _settlementsFile = "Data/final_settlements.json";
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public OffboardingService()
    {
        Directory.CreateDirectory("Data");
    }

    public async Task<List<Resignation>> GetAllResignationsAsync()
    {
        return await ReadFromFileAsync<Resignation>(_resignationsFile);
    }

    public async Task<Resignation?> GetResignationByIdAsync(int id)
    {
        var resignations = await GetAllResignationsAsync();
        return resignations.FirstOrDefault(r => r.Id == id);
    }

    public async Task<Resignation> SubmitResignationAsync(Resignation resignation)
    {
        var resignations = await GetAllResignationsAsync();
        resignation.Id = resignations.Any() ? resignations.Max(r => r.Id) + 1 : 1;
        resignation.SubmissionDate = DateTime.UtcNow;
        resignation.Status = ResignationStatus.Submitted;
        resignations.Add(resignation);
        await SaveToFileAsync(_resignationsFile, resignations);
        return resignation;
    }

    public async Task<Resignation?> UpdateResignationStatusAsync(int id, ResignationStatus status)
    {
        var resignations = await GetAllResignationsAsync();
        var resignation = resignations.FirstOrDefault(r => r.Id == id);
        if (resignation == null) return null;

        resignation.Status = status;
        await SaveToFileAsync(_resignationsFile, resignations);
        return resignation;
    }

    public async Task<List<Termination>> GetAllTerminationsAsync()
    {
        return await ReadFromFileAsync<Termination>(_terminationsFile);
    }

    public async Task<Termination?> GetTerminationByIdAsync(int id)
    {
        var terminations = await GetAllTerminationsAsync();
        return terminations.FirstOrDefault(t => t.Id == id);
    }

    public async Task<Termination> CreateTerminationAsync(Termination termination)
    {
        var terminations = await GetAllTerminationsAsync();
        termination.Id = terminations.Any() ? terminations.Max(t => t.Id) + 1 : 1;
        terminations.Add(termination);
        await SaveToFileAsync(_terminationsFile, terminations);
        return termination;
    }

    public async Task<List<ExitInterview>> GetAllExitInterviewsAsync()
    {
        return await ReadFromFileAsync<ExitInterview>(_exitInterviewsFile);
    }

    public async Task<ExitInterview?> GetExitInterviewByIdAsync(int id)
    {
        var interviews = await GetAllExitInterviewsAsync();
        return interviews.FirstOrDefault(i => i.Id == id);
    }

    public async Task<ExitInterview> CreateExitInterviewAsync(ExitInterview interview)
    {
        var interviews = await GetAllExitInterviewsAsync();
        interview.Id = interviews.Any() ? interviews.Max(i => i.Id) + 1 : 1;
        interviews.Add(interview);
        await SaveToFileAsync(_exitInterviewsFile, interviews);
        return interview;
    }

    public async Task<ExitInterview?> UpdateExitInterviewAsync(int id, ExitInterview interview)
    {
        var interviews = await GetAllExitInterviewsAsync();
        var existing = interviews.FirstOrDefault(i => i.Id == id);
        if (existing == null) return null;

        var index = interviews.IndexOf(existing);
        interview.Id = id;
        interviews[index] = interview;
        await SaveToFileAsync(_exitInterviewsFile, interviews);
        return interview;
    }

    public async Task<List<ExitClearance>> GetAllClearancesAsync()
    {
        return await ReadFromFileAsync<ExitClearance>(_clearancesFile);
    }

    public async Task<ExitClearance?> GetClearanceByEmployeeIdAsync(int employeeId)
    {
        var clearances = await GetAllClearancesAsync();
        return clearances.FirstOrDefault(c => c.EmployeeId == employeeId);
    }

    public async Task<ExitClearance> InitiateClearanceAsync(ExitClearance clearance)
    {
        var clearances = await GetAllClearancesAsync();
        clearance.Id = clearances.Any() ? clearances.Max(c => c.Id) + 1 : 1;
        clearance.InitiatedDate = DateTime.UtcNow;
        clearance.IsFullyCleared = false;
        clearances.Add(clearance);
        await SaveToFileAsync(_clearancesFile, clearances);
        return clearance;
    }

    public async Task<ExitClearance?> UpdateClearanceItemAsync(int clearanceId, int itemIndex, bool isCleared, string clearedBy)
    {
        var clearances = await GetAllClearancesAsync();
        var clearance = clearances.FirstOrDefault(c => c.Id == clearanceId);
        if (clearance == null || itemIndex >= clearance.Items.Count) return null;

        clearance.Items[itemIndex].IsCleared = isCleared;
        clearance.Items[itemIndex].ClearedDate = DateTime.UtcNow;
        clearance.Items[itemIndex].ClearedBy = clearedBy;
        
        clearance.IsFullyCleared = clearance.Items.All(i => i.IsCleared);
        if (clearance.IsFullyCleared) clearance.CompletedDate = DateTime.UtcNow;

        await SaveToFileAsync(_clearancesFile, clearances);
        return clearance;
    }

    public async Task<List<FinalSettlement>> GetAllSettlementsAsync()
    {
        return await ReadFromFileAsync<FinalSettlement>(_settlementsFile);
    }

    public async Task<FinalSettlement?> GetSettlementByEmployeeIdAsync(int employeeId)
    {
        var settlements = await GetAllSettlementsAsync();
        return settlements.FirstOrDefault(s => s.EmployeeId == employeeId);
    }

    public async Task<FinalSettlement> CalculateSettlementAsync(FinalSettlement settlement)
    {
        var settlements = await GetAllSettlementsAsync();
        settlement.Id = settlements.Any() ? settlements.Max(s => s.Id) + 1 : 1;
        settlement.CalculationDate = DateTime.UtcNow;
        settlement.TotalPayable = settlement.OutstandingSalary + settlement.UnusedLeaveEncashment + 
                                  settlement.Bonus - settlement.Deductions;
        settlement.Status = SettlementStatus.Calculated;
        settlements.Add(settlement);
        await SaveToFileAsync(_settlementsFile, settlements);
        return settlement;
    }

    public async Task<FinalSettlement?> ProcessPaymentAsync(int id, string paymentMode)
    {
        var settlements = await GetAllSettlementsAsync();
        var settlement = settlements.FirstOrDefault(s => s.Id == id);
        if (settlement == null) return null;

        settlement.PaymentDate = DateTime.UtcNow;
        settlement.PaymentMode = paymentMode;
        settlement.Status = SettlementStatus.Paid;
        await SaveToFileAsync(_settlementsFile, settlements);
        return settlement;
    }

    public async Task<object> GetOffboardingStatsAsync()
    {
        var resignations = await GetAllResignationsAsync();
        var terminations = await GetAllTerminationsAsync();
        var interviews = await GetAllExitInterviewsAsync();

        return new
        {
            TotalResignations = resignations.Count,
            PendingResignations = resignations.Count(r => r.Status == ResignationStatus.Submitted),
            TotalTerminations = terminations.Count,
            ExitInterviewsCompleted = interviews.Count(i => i.Status == ExitInterviewStatus.Completed),
            AverageSatisfactionScore = interviews.Where(i => i.OverallSatisfaction.HasValue)
                .Select(i => i.OverallSatisfaction!.Value).DefaultIfEmpty(0).Average()
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
