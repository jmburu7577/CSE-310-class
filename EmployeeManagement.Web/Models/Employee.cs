namespace EmployeeManagement.Web.Models;

/// <summary>
/// Represents an employee in the system
/// </summary>
public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Department { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public Address Address { get; set; } = new();

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
