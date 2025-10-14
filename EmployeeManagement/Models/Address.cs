namespace EmployeeManagement.Models;

public struct Address
{
    public string Street { get; set; }
    public string City   { get; set; }
    public string State  { get; set; }
    public string Zip    { get; set; }

    public Address(string street, string city, string state, string zip)
    {
        Street = street;
        City = city;
        State = state;
        Zip = zip;
    }

    public override string ToString()
        => $"{Street}, {City}, {State} {Zip}";
}
