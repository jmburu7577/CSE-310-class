using EmployeeManagement.Web.Models;
using System.Text.Json;

namespace EmployeeManagement.Web.Services;

public class AssetService : IAssetService
{
    private readonly string _assetsFile = "Data/company_assets.json";
    private readonly string _assignmentsFile = "Data/asset_assignments.json";
    private readonly string _maintenanceFile = "Data/asset_maintenance.json";
    private readonly string _disposalsFile = "Data/asset_disposals.json";
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public AssetService()
    {
        Directory.CreateDirectory("Data");
    }

    // Assets
    public async Task<List<CompanyAsset>> GetAllAssetsAsync()
    {
        return await ReadFromFileAsync<CompanyAsset>(_assetsFile);
    }

    public async Task<CompanyAsset?> GetAssetByIdAsync(int id)
    {
        var assets = await GetAllAssetsAsync();
        return assets.FirstOrDefault(a => a.Id == id);
    }

    public async Task<List<CompanyAsset>> GetAssetsByEmployeeAsync(int employeeId)
    {
        var assets = await GetAllAssetsAsync();
        return assets.Where(a => a.AssignedToEmployeeId == employeeId && a.Status == AssetStatus.Assigned).ToList();
    }

    public async Task<CompanyAsset> CreateAssetAsync(CompanyAsset asset)
    {
        var assets = await GetAllAssetsAsync();
        asset.Id = assets.Any() ? assets.Max(a => a.Id) + 1 : 1;
        asset.Status = AssetStatus.Available;
        assets.Add(asset);
        await SaveToFileAsync(_assetsFile, assets);
        return asset;
    }

    public async Task<CompanyAsset?> UpdateAssetAsync(int id, CompanyAsset asset)
    {
        var assets = await GetAllAssetsAsync();
        var existing = assets.FirstOrDefault(a => a.Id == id);
        if (existing == null) return null;

        var index = assets.IndexOf(existing);
        asset.Id = id;
        assets[index] = asset;
        await SaveToFileAsync(_assetsFile, assets);
        return asset;
    }

    public async Task<bool> DeleteAssetAsync(int id)
    {
        var assets = await GetAllAssetsAsync();
        var asset = assets.FirstOrDefault(a => a.Id == id);
        if (asset == null) return false;

        assets.Remove(asset);
        await SaveToFileAsync(_assetsFile, assets);
        return true;
    }

    // Assignments
    public async Task<List<AssetAssignment>> GetAllAssignmentsAsync()
    {
        return await ReadFromFileAsync<AssetAssignment>(_assignmentsFile);
    }

    public async Task<List<AssetAssignment>> GetEmployeeAssignmentsAsync(int employeeId)
    {
        var assignments = await GetAllAssignmentsAsync();
        return assignments.Where(a => a.EmployeeId == employeeId).OrderByDescending(a => a.AssignedDate).ToList();
    }

    public async Task<AssetAssignment> AssignAssetAsync(AssetAssignment assignment)
    {
        var assignments = await GetAllAssignmentsAsync();
        assignment.Id = assignments.Any() ? assignments.Max(a => a.Id) + 1 : 1;
        assignment.AssignedDate = DateTime.UtcNow;
        assignment.IsActive = true;
        assignments.Add(assignment);
        await SaveToFileAsync(_assignmentsFile, assignments);

        // Update asset status
        var assets = await GetAllAssetsAsync();
        var asset = assets.FirstOrDefault(a => a.Id == assignment.AssetId);
        if (asset != null)
        {
            asset.Status = AssetStatus.Assigned;
            asset.AssignedToEmployeeId = assignment.EmployeeId;
            asset.AssignmentDate = DateTime.UtcNow;
            await SaveToFileAsync(_assetsFile, assets);
        }

        return assignment;
    }

    public async Task<AssetAssignment?> ReturnAssetAsync(int assetId, AssetCondition condition, string notes)
    {
        var assignments = await GetAllAssignmentsAsync();
        var assignment = assignments.FirstOrDefault(a => a.AssetId == assetId && a.IsActive);
        if (assignment == null) return null;

        assignment.ReturnedDate = DateTime.UtcNow;
        assignment.ConditionOnReturn = condition;
        assignment.ReturnNotes = notes;
        assignment.IsActive = false;
        await SaveToFileAsync(_assignmentsFile, assignments);

        // Update asset status
        var assets = await GetAllAssetsAsync();
        var asset = assets.FirstOrDefault(a => a.Id == assetId);
        if (asset != null)
        {
            asset.Status = AssetStatus.Available;
            asset.Condition = condition;
            asset.AssignedToEmployeeId = null;
            asset.AssignmentDate = null;
            await SaveToFileAsync(_assetsFile, assets);
        }

        return assignment;
    }

    // Maintenance
    public async Task<List<AssetMaintenance>> GetAssetMaintenanceHistoryAsync(int assetId)
    {
        var allMaintenance = await ReadFromFileAsync<AssetMaintenance>(_maintenanceFile);
        return allMaintenance.Where(m => m.AssetId == assetId).OrderByDescending(m => m.MaintenanceDate).ToList();
    }

    public async Task<AssetMaintenance> AddMaintenanceRecordAsync(AssetMaintenance maintenance)
    {
        var allMaintenance = await ReadFromFileAsync<AssetMaintenance>(_maintenanceFile);
        maintenance.Id = allMaintenance.Any() ? allMaintenance.Max(m => m.Id) + 1 : 1;
        allMaintenance.Add(maintenance);
        await SaveToFileAsync(_maintenanceFile, allMaintenance);

        // Update asset status to under maintenance
        var assets = await GetAllAssetsAsync();
        var asset = assets.FirstOrDefault(a => a.Id == maintenance.AssetId);
        if (asset != null)
        {
            asset.Status = AssetStatus.UnderMaintenance;
            await SaveToFileAsync(_assetsFile, assets);
        }

        return maintenance;
    }

    // Disposal
    public async Task<List<AssetDisposal>> GetAllDisposalsAsync()
    {
        return await ReadFromFileAsync<AssetDisposal>(_disposalsFile);
    }

    public async Task<AssetDisposal> DisposeAssetAsync(AssetDisposal disposal)
    {
        var disposals = await ReadFromFileAsync<AssetDisposal>(_disposalsFile);
        disposal.Id = disposals.Any() ? disposals.Max(d => d.Id) + 1 : 1;
        disposal.DisposalDate = DateTime.UtcNow;
        disposals.Add(disposal);
        await SaveToFileAsync(_disposalsFile, disposals);

        // Update asset status
        var assets = await GetAllAssetsAsync();
        var asset = assets.FirstOrDefault(a => a.Id == disposal.AssetId);
        if (asset != null)
        {
            asset.Status = AssetStatus.Disposed;
            await SaveToFileAsync(_assetsFile, assets);
        }

        return disposal;
    }

    // Inventory
    public async Task<InventoryOverview> GetInventoryOverviewAsync()
    {
        var assets = await GetAllAssetsAsync();

        return new InventoryOverview
        {
            TotalAssets = assets.Count,
            AssignedAssets = assets.Count(a => a.Status == AssetStatus.Assigned),
            AvailableAssets = assets.Count(a => a.Status == AssetStatus.Available),
            UnderMaintenance = assets.Count(a => a.Status == AssetStatus.UnderMaintenance),
            Disposed = assets.Count(a => a.Status == AssetStatus.Disposed),
            AssetsByType = assets.GroupBy(a => a.AssetType).ToDictionary(g => g.Key, g => g.Count()),
            AssetsByCondition = assets.GroupBy(a => a.Condition).ToDictionary(g => g.Key, g => g.Count()),
            TotalValue = assets.Where(a => a.Status != AssetStatus.Disposed).Sum(a => a.PurchaseValue)
        };
    }

    // Statistics
    public async Task<object> GetAssetStatsAsync()
    {
        var overview = await GetInventoryOverviewAsync();
        var assignments = await GetAllAssignmentsAsync();
        var maintenance = await ReadFromFileAsync<AssetMaintenance>(_maintenanceFile);

        return new
        {
            Overview = overview,
            TotalAssignments = assignments.Count,
            ActiveAssignments = assignments.Count(a => a.IsActive),
            MaintenanceRecords = maintenance.Count,
            MaintenanceCost = maintenance.Sum(m => m.Cost)
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
