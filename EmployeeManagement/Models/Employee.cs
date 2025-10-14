namespace EmployeeManagement.Models;

public class Employee : Person
{
    public int Id { get; set; }
    public string Department { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public Address Address { get; set; }

    public Employee() { }

    public Employee(int id, string firstName, string lastName, int age, string department, decimal salary, Address address)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Department = department;
        Salary = salary;
        Address = address;
    }

    public override string GetDisplayName() => $"{FirstName} {LastName}";
}
