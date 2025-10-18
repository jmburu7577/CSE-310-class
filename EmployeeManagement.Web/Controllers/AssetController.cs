using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Services;

namespace EmployeeManagement.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AssetController : ControllerBase
{
    private readonly IAssetService _service;

    public AssetController(IAssetService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<CompanyAsset>>> GetAllAssets() => 
        Ok(await _service.GetAllAssetsAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyAsset>> GetAsset(int id)
    {
        var asset = await _service.GetAssetByIdAsync(id);
        return asset == null ? NotFound() : Ok(asset);
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<List<CompanyAsset>>> GetEmployeeAssets(int employeeId) => 
        Ok(await _service.GetAssetsByEmployeeAsync(employeeId));

    [HttpPost]
    public async Task<ActionResult<CompanyAsset>> CreateAsset(CompanyAsset asset) => 
        Ok(await _service.CreateAssetAsync(asset));

    [HttpPut("{id}")]
    public async Task<ActionResult<CompanyAsset>> UpdateAsset(int id, CompanyAsset asset)
    {
        var updated = await _service.UpdateAssetAsync(id, asset);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsset(int id) => 
        await _service.DeleteAssetAsync(id) ? NoContent() : NotFound();

    [HttpGet("assignments")]
    public async Task<ActionResult<List<AssetAssignment>>> GetAllAssignments() => 
        Ok(await _service.GetAllAssignmentsAsync());

    [HttpGet("assignments/employee/{employeeId}")]
    public async Task<ActionResult<List<AssetAssignment>>> GetEmployeeAssignments(int employeeId) => 
        Ok(await _service.GetEmployeeAssignmentsAsync(employeeId));

    [HttpPost("assign")]
    public async Task<ActionResult<AssetAssignment>> AssignAsset(AssetAssignment assignment) => 
        Ok(await _service.AssignAssetAsync(assignment));

    [HttpPost("{assetId}/return")]
    public async Task<ActionResult<AssetAssignment>> ReturnAsset(int assetId, 
        AssetCondition condition, string notes)
    {
        var assignment = await _service.ReturnAssetAsync(assetId, condition, notes);
        return assignment == null ? NotFound() : Ok(assignment);
    }

    [HttpGet("{assetId}/maintenance")]
    public async Task<ActionResult<List<AssetMaintenance>>> GetMaintenanceHistory(int assetId) => 
        Ok(await _service.GetAssetMaintenanceHistoryAsync(assetId));

    [HttpPost("maintenance")]
    public async Task<ActionResult<AssetMaintenance>> AddMaintenanceRecord(AssetMaintenance maintenance) => 
        Ok(await _service.AddMaintenanceRecordAsync(maintenance));

    [HttpGet("disposals")]
    public async Task<ActionResult<List<AssetDisposal>>> GetDisposals() => 
        Ok(await _service.GetAllDisposalsAsync());

    [HttpPost("dispose")]
    public async Task<ActionResult<AssetDisposal>> DisposeAsset(AssetDisposal disposal) => 
        Ok(await _service.DisposeAssetAsync(disposal));

    [HttpGet("inventory")]
    public async Task<ActionResult<InventoryOverview>> GetInventory() => 
        Ok(await _service.GetInventoryOverviewAsync());

    [HttpGet("stats")]
    public async Task<ActionResult<object>> GetStats() => Ok(await _service.GetAssetStatsAsync());
}
