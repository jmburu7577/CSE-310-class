namespace EmployeeManagement.Web.Models;

/// <summary>
/// Represents an employee in the system
/// </summary>
public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public int? ManagerId { get; set; } // Reference to manager employee ID
    public decimal Salary { get; set; }
    public DateTime JoinDate { get; set; }
    public Address Address { get; set; } = new();
    public EmergencyContact EmergencyContact { get; set; } = new();
    public List<EmployeeDocument> Documents { get; set; } = new();

    public string GetDisplayName() => $"{FirstName} {LastName}";
}

/// <summary>
/// Represents an employee's address
/// </summary>
public class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;

    public override string ToString()
        => $"{Street}, {City}, {State} {Zip}";
}

/// <summary>
/// Represents an emergency contact for an employee
/// </summary>
public class EmergencyContact
{
    public string Name { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Represents a document stored for an employee
/// </summary>
public class EmployeeDocument
{
    public int Id { get; set; }
    public string DocumentType { get; set; } = string.Empty; // ID, Certificate, Offer Letter, etc.
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public DateTime UploadDate { get; set; }
    public string UploadedBy { get; set; } = string.Empty;
}
