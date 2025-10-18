using EmployeeManagement.Web.Models;

namespace EmployeeManagement.Web.Services;

public interface IAssetService
{
    // Assets
    Task<List<CompanyAsset>> GetAllAssetsAsync();
    Task<CompanyAsset?> GetAssetByIdAsync(int id);
    Task<List<CompanyAsset>> GetAssetsByEmployeeAsync(int employeeId);
    Task<CompanyAsset> CreateAssetAsync(CompanyAsset asset);
    Task<CompanyAsset?> UpdateAssetAsync(int id, CompanyAsset asset);
    Task<bool> DeleteAssetAsync(int id);
    
    // Assignments
    Task<List<AssetAssignment>> GetAllAssignmentsAsync();
    Task<List<AssetAssignment>> GetEmployeeAssignmentsAsync(int employeeId);
    Task<AssetAssignment> AssignAssetAsync(AssetAssignment assignment);
    Task<AssetAssignment?> ReturnAssetAsync(int assetId, AssetCondition condition, string notes);
    
    // Maintenance
    Task<List<AssetMaintenance>> GetAssetMaintenanceHistoryAsync(int assetId);
    Task<AssetMaintenance> AddMaintenanceRecordAsync(AssetMaintenance maintenance);
    
    // Disposal
    Task<List<AssetDisposal>> GetAllDisposalsAsync();
    Task<AssetDisposal> DisposeAssetAsync(AssetDisposal disposal);
    
    // Inventory
    Task<InventoryOverview> GetInventoryOverviewAsync();
    
    // Statistics
    Task<object> GetAssetStatsAsync();
}
