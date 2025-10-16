using System.Text.Json;
using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

/// <summary>
/// Service for managing leave requests and balances
/// </summary>
public class LeaveService : ILeaveService
{
    private readonly List<LeaveRequest> _leaveRequests = new();
    private readonly List<LeaveBalance> _leaveBalances = new();
    private readonly string _requestsFile;
    private readonly string _balancesFile;
    private readonly SemaphoreSlim _fileLock = new(1, 1);
    private readonly IEmployeeService _employeeService;
    private readonly IAuthService _authService;
    private readonly ILogger<LeaveService> _logger;
    private int _nextId = 1;

    // Default leave allocations per year
    private const decimal DEFAULT_VACATION_DAYS = 20m;
    private const decimal DEFAULT_SICK_DAYS = 10m;
    private const decimal DEFAULT_PERSONAL_DAYS = 5m;

    public LeaveService(
        IEmployeeService employeeService,
        IAuthService authService,
        ILogger<LeaveService> logger)
    {
        _employeeService = employeeService;
        _authService = authService;
        _logger = logger;
        _requestsFile = Path.Combine(AppContext.BaseDirectory, "leave_requests.json");
        _balancesFile = Path.Combine(AppContext.BaseDirectory, "leave_balances.json");
        LoadDataAsync().Wait();
    }

    #region Leave Requests

    public async Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync()
    {
        await Task.CompletedTask;
        return _leaveRequests.OrderByDescending(l => l.RequestedDate).ToList();
    }

    public async Task<LeaveRequest?> GetLeaveRequestByIdAsync(int id)
    {
        await Task.CompletedTask;
        return _leaveRequests.FirstOrDefault(l => l.Id == id);
    }

    public async Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByEmployeeIdAsync(int employeeId)
    {
        await Task.CompletedTask;
        return _leaveRequests
            .Where(l => l.EmployeeId == employeeId)
            .OrderByDescending(l => l.RequestedDate)
            .ToList();
    }

    public async Task<IEnumerable<LeaveRequest>> GetPendingLeaveRequestsAsync()
    {
        await Task.CompletedTask;
        return _leaveRequests
            .Where(l => l.Status == LeaveStatus.Pending)
            .OrderBy(l => l.StartDate)
            .ToList();
    }

    public async Task<LeaveRequest> CreateLeaveRequestAsync(int employeeId, CreateLeaveRequest request)
    {
        // Validate employee exists
        var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
        if (employee == null)
        {
            throw new InvalidOperationException($"Employee with ID {employeeId} not found");
        }

        // Validate dates
        if (request.StartDate < DateTime.Today)
        {
            throw new InvalidOperationException("Start date cannot be in the past");
        }

        if (request.EndDate < request.StartDate)
        {
            throw new InvalidOperationException("End date must be after start date");
        }

        // Calculate business days
        var days = CalculateBusinessDays(request.StartDate, request.EndDate);

        // Check leave balance for certain leave types
        var year = request.StartDate.Year;
        var balance = await GetLeaveBalanceAsync(employeeId, year);
        
        if (balance == null)
        {
            balance = await InitializeLeaveBalanceAsync(employeeId, year);
        }

        // Validate sufficient balance for paid leave types
        if (request.Type == LeaveType.Vacation && balance.VacationRemaining < days)
        {
            throw new InvalidOperationException($"Insufficient vacation days. Available: {balance.VacationRemaining}, Requested: {days}");
        }
        else if (request.Type == LeaveType.Sick && balance.SickRemaining < days)
        {
            throw new InvalidOperationException($"Insufficient sick days. Available: {balance.SickRemaining}, Requested: {days}");
        }
        else if (request.Type == LeaveType.Personal && balance.PersonalRemaining < days)
        {
            throw new InvalidOperationException($"Insufficient personal days. Available: {balance.PersonalRemaining}, Requested: {days}");
        }

        // Create leave request
        var leaveRequest = new LeaveRequest
        {
            Id = _nextId++,
            EmployeeId = employeeId,
            Type = request.Type,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Days = days,
            Reason = request.Reason,
            Status = LeaveStatus.Pending,
            RequestedDate = DateTime.UtcNow
        };

        _leaveRequests.Add(leaveRequest);
        await SaveRequestsAsync();

        return leaveRequest;
    }

    public async Task<LeaveRequest?> ApproveLeaveRequestAsync(int leaveId, int approverId, ApproveLeaveRequest request)
    {
        var leaveRequest = await GetLeaveRequestByIdAsync(leaveId);
        if (leaveRequest == null)
        {
            return null;
        }

        if (leaveRequest.Status != LeaveStatus.Pending)
        {
            throw new InvalidOperationException($"Leave request is already {leaveRequest.Status}");
        }

        leaveRequest.Status = LeaveStatus.Approved;
        leaveRequest.ApprovedBy = approverId;
        leaveRequest.ApprovedDate = DateTime.UtcNow;
        leaveRequest.ApproverNotes = request.Notes;

        // Update leave balance
        await UpdateLeaveBalanceAsync(leaveRequest.EmployeeId, leaveRequest.Type, leaveRequest.Days, true);
        
        await SaveRequestsAsync();
        return leaveRequest;
    }

    public async Task<LeaveRequest?> RejectLeaveRequestAsync(int leaveId, int approverId, ApproveLeaveRequest request)
    {
        var leaveRequest = await GetLeaveRequestByIdAsync(leaveId);
        if (leaveRequest == null)
        {
            return null;
        }

        if (leaveRequest.Status != LeaveStatus.Pending)
        {
            throw new InvalidOperationException($"Leave request is already {leaveRequest.Status}");
        }

        leaveRequest.Status = LeaveStatus.Rejected;
        leaveRequest.ApprovedBy = approverId;
        leaveRequest.ApprovedDate = DateTime.UtcNow;
        leaveRequest.ApproverNotes = request.Notes;

        await SaveRequestsAsync();
        return leaveRequest;
    }

    public async Task<bool> CancelLeaveRequestAsync(int leaveId, int employeeId)
    {
        var leaveRequest = await GetLeaveRequestByIdAsync(leaveId);
        if (leaveRequest == null)
        {
            return false;
        }

        if (leaveRequest.EmployeeId != employeeId)
        {
            throw new InvalidOperationException("You can only cancel your own leave requests");
        }

        if (leaveRequest.Status != LeaveStatus.Pending)
        {
            throw new InvalidOperationException($"Cannot cancel a {leaveRequest.Status} leave request");
        }

        leaveRequest.Status = LeaveStatus.Cancelled;
        await SaveRequestsAsync();
        return true;
    }

    public async Task<bool> DeleteLeaveRequestAsync(int id)
    {
        var leaveRequest = _leaveRequests.FirstOrDefault(l => l.Id == id);
        if (leaveRequest == null)
        {
            return false;
        }

        // If approved, restore the balance
        if (leaveRequest.Status == LeaveStatus.Approved)
        {
            await UpdateLeaveBalanceAsync(leaveRequest.EmployeeId, leaveRequest.Type, leaveRequest.Days, false);
        }

        _leaveRequests.Remove(leaveRequest);
        await SaveRequestsAsync();
        return true;
    }

    #endregion

    #region Leave Balances

    public async Task<LeaveBalance?> GetLeaveBalanceAsync(int employeeId, int year)
    {
        await Task.CompletedTask;
        return _leaveBalances.FirstOrDefault(b => b.EmployeeId == employeeId && b.Year == year);
    }

    public async Task<LeaveBalance> InitializeLeaveBalanceAsync(int employeeId, int year)
    {
        var existing = await GetLeaveBalanceAsync(employeeId, year);
        if (existing != null)
        {
            return existing;
        }

        var balance = new LeaveBalance
        {
            EmployeeId = employeeId,
            Year = year,
            VacationDays = DEFAULT_VACATION_DAYS,
            SickDays = DEFAULT_SICK_DAYS,
            PersonalDays = DEFAULT_PERSONAL_DAYS,
            VacationUsed = 0,
            SickUsed = 0,
            PersonalUsed = 0
        };

        _leaveBalances.Add(balance);
        await SaveBalancesAsync();
        return balance;
    }

    public async Task UpdateLeaveBalanceAsync(int employeeId, LeaveType leaveType, decimal days, bool isUsed)
    {
        var year = DateTime.Now.Year;
        var balance = await GetLeaveBalanceAsync(employeeId, year);

        if (balance == null)
        {
            balance = await InitializeLeaveBalanceAsync(employeeId, year);
        }

        // Update used days based on leave type
        switch (leaveType)
        {
            case LeaveType.Vacation:
                balance.VacationUsed += isUsed ? days : -days;
                break;
            case LeaveType.Sick:
                balance.SickUsed += isUsed ? days : -days;
                break;
            case LeaveType.Personal:
                balance.PersonalUsed += isUsed ? days : -days;
                break;
            // Unpaid, Maternity, Paternity, Bereavement, Study don't affect balance
        }

        await SaveBalancesAsync();
    }

    #endregion

    #region Statistics

    public async Task<LeaveStatistics> GetLeaveStatisticsAsync()
    {
        var stats = new LeaveStatistics
        {
            TotalRequests = _leaveRequests.Count,
            PendingRequests = _leaveRequests.Count(l => l.Status == LeaveStatus.Pending),
            ApprovedRequests = _leaveRequests.Count(l => l.Status == LeaveStatus.Approved),
            RejectedRequests = _leaveRequests.Count(l => l.Status == LeaveStatus.Rejected),
        };

        // Count by leave type
        foreach (LeaveType type in Enum.GetValues(typeof(LeaveType)))
        {
            stats.RequestsByType[type] = _leaveRequests.Count(l => l.Type == type);
        }

        // Recent requests with employee details
        var recentRequests = _leaveRequests
            .OrderByDescending(l => l.RequestedDate)
            .Take(10)
            .ToList();

        foreach (var request in recentRequests)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(request.EmployeeId);
            User? approver = null;
            if (request.ApprovedBy.HasValue)
            {
                approver = await _authService.GetUserByIdAsync(request.ApprovedBy.Value);
            }

            if (employee != null)
            {
                stats.RecentRequests.Add(new LeaveRequestWithEmployee
                {
                    LeaveRequest = request,
                    Employee = employee,
                    Approver = approver
                });
            }
        }

        return stats;
    }

    #endregion

    #region Helper Methods

    private int CalculateBusinessDays(DateTime startDate, DateTime endDate)
    {
        int businessDays = 0;
        var currentDate = startDate;

        while (currentDate <= endDate)
        {
            // Count weekdays only (Monday to Friday)
            if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
            {
                businessDays++;
            }
            currentDate = currentDate.AddDays(1);
        }

        return businessDays;
    }

    private async Task SaveRequestsAsync()
    {
        await _fileLock.WaitAsync();
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_leaveRequests, options);
            await File.WriteAllTextAsync(_requestsFile, json);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private async Task SaveBalancesAsync()
    {
        await _fileLock.WaitAsync();
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_leaveBalances, options);
            await File.WriteAllTextAsync(_balancesFile, json);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private async Task LoadDataAsync()
    {
        await LoadRequestsAsync();
        await LoadBalancesAsync();
    }

    private async Task LoadRequestsAsync()
    {
        if (!File.Exists(_requestsFile))
        {
            return;
        }

        await _fileLock.WaitAsync();
        try
        {
            var json = await File.ReadAllTextAsync(_requestsFile);
            var data = JsonSerializer.Deserialize<List<LeaveRequest>>(json) ?? new List<LeaveRequest>();
            _leaveRequests.Clear();
            _leaveRequests.AddRange(data);

            _nextId = _leaveRequests.Any() ? _leaveRequests.Max(l => l.Id) + 1 : 1;
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private async Task LoadBalancesAsync()
    {
        if (!File.Exists(_balancesFile))
        {
            return;
        }

        await _fileLock.WaitAsync();
        try
        {
            var json = await File.ReadAllTextAsync(_balancesFile);
            var data = JsonSerializer.Deserialize<List<LeaveBalance>>(json) ?? new List<LeaveBalance>();
            _leaveBalances.Clear();
            _leaveBalances.AddRange(data);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    #endregion
}
