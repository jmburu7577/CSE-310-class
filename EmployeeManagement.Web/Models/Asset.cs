namespace EmployeeManagement.Web.Models;

/// <summary>
/// Represents a company asset
/// </summary>
public class CompanyAsset
{
    public int Id { get; set; }
    public string AssetType { get; set; } = string.Empty; // Laptop, Phone, ID Badge, etc.
    public string AssetTag { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public decimal PurchaseValue { get; set; }
    public AssetStatus Status { get; set; }
    public AssetCondition Condition { get; set; }
    public string Location { get; set; } = string.Empty;
    public int? AssignedToEmployeeId { get; set; }
    public DateTime? AssignmentDate { get; set; }
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// Represents an asset assignment history
/// </summary>
public class AssetAssignment
{
    public int Id { get; set; }
    public int AssetId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime AssignedDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public AssetCondition ConditionOnAssignment { get; set; }
    public AssetCondition? ConditionOnReturn { get; set; }
    public string AssignedBy { get; set; } = string.Empty;
    public string ReturnNotes { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

/// <summary>
/// Represents an asset maintenance record
/// </summary>
public class AssetMaintenance
{
    public int Id { get; set; }
    public int AssetId { get; set; }
    public DateTime MaintenanceDate { get; set; }
    public string MaintenanceType { get; set; } = string.Empty; // Repair, Upgrade, Cleaning
    public string Description { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public string PerformedBy { get; set; } = string.Empty;
    public string Vendor { get; set; } = string.Empty;
    public DateTime? NextMaintenanceDate { get; set; }
}

/// <summary>
/// Represents an asset disposal record
/// </summary>
public class AssetDisposal
{
    public int Id { get; set; }
    public int AssetId { get; set; }
    public DateTime DisposalDate { get; set; }
    public DisposalMethod Method { get; set; }
    public string Reason { get; set; } = string.Empty;
    public decimal SalvageValue { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// Represents an inventory overview
/// </summary>
public class InventoryOverview
{
    public int TotalAssets { get; set; }
    public int AssignedAssets { get; set; }
    public int AvailableAssets { get; set; }
    public int UnderMaintenance { get; set; }
    public int Disposed { get; set; }
    public Dictionary<string, int> AssetsByType { get; set; } = new();
    public Dictionary<AssetCondition, int> AssetsByCondition { get; set; } = new();
    public decimal TotalValue { get; set; }
}

// Enums
public enum AssetStatus
{
    Available,
    Assigned,
    UnderMaintenance,
    Damaged,
    Lost,
    Disposed
}

public enum AssetCondition
{
    New,
    Good,
    Fair,
    Poor,
    Damaged,
    BeyondRepair
}

public enum DisposalMethod
{
    Sold,
    Donated,
    Recycled,
    Scrapped,
    Lost
}
