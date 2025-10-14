using System.Text.Json;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using EmployeeManagement.Utils;
using Spectre.Console;

namespace EmployeeManagement;

public class Program
{
    private static readonly string DataFile = Path.Combine(AppContext.BaseDirectory, "employees.json");

    public static void Main(string[] args)
    {
        var repo = new EmployeeRepository();
        // Try loading existing data (demonstrates file read)
        if (File.Exists(DataFile))
        {
            try
            {
                repo.LoadFromFile(DataFile);
                AnsiConsole.MarkupLine($"[green]Loaded[/] [bold]{repo.Employees.Count}[/] employees from file.\n");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[yellow]Warning[/]: Failed to load existing data: [red]{Markup.Escape(ex.Message)}[/]");
            }
        }

        bool exit = false; // Variable + boolean expression usage
        while (!exit) // Loop + conditional
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Employee Manager").Centered().Color(Color.Cyan1));
            AnsiConsole.Write(new Rule("Main Menu").RuleStyle("grey50").Centered());

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Select an option[/]:")
                    .HighlightStyle("bold cyan1")
                    .AddChoices(
                        "Add Employee",
                        "Edit Employee",
                        "List Employees",
                        "Save to File",
                        "Load from File",
                        "Exit"));

            switch (choice)
            {
                case "Add Employee":
                    AddEmployee(repo);
                    break;
                case "Edit Employee":
                    EditEmployee(repo);
                    break;
                case "List Employees":
                    ListEmployees(repo);
                    break;
                case "Save to File":
                    Save(repo);
                    break;
                case "Load from File":
                    Load(repo);
                    break;
                case "Exit":
                    exit = true;
                    break;
            }

            if (!exit)
            {
                AnsiConsole.MarkupLine("\n[grey50]Press any key to return to the menu...[/]");
                Console.ReadKey(true);
            }
        }

        AnsiConsole.MarkupLine("[green]Goodbye![/]");
    }

    // Functions demonstrating input and creation of objects
    private static void AddEmployee(EmployeeRepository repo)
    {
        AnsiConsole.Write(new Panel("Add a new employee").Border(BoxBorder.Rounded).BorderStyle("cyan1").Header("[bold]Add Employee[/]"));
        int id = InputHelper.ReadInt("ID");
        string firstName = InputHelper.ReadString("First name");
        string lastName = InputHelper.ReadString("Last name");
        int age = InputHelper.ReadInt("Age");
        string department = InputHelper.ReadString("Department");
        decimal salary = InputHelper.ReadDecimal("Salary");

        AnsiConsole.Write(new Rule("Address").RuleStyle("grey50"));
        string street = InputHelper.ReadString("Street");
        string city = InputHelper.ReadString("City");
        string state = InputHelper.ReadString("State");
        string zip = InputHelper.ReadString("Zip");

        var address = new Address(street, city, state, zip);
        Employee employee = new Employee(id, firstName, lastName, age, department, salary, address);

        repo.AddEmployee(employee);
        AnsiConsole.MarkupLine($"[green]Added[/]: [bold]{Markup.Escape(employee.GetDisplayName())}[/] (ID: [bold]{employee.Id}[/])\n");
    }

    private static void EditEmployee(EmployeeRepository repo)
    {
        AnsiConsole.Write(new Panel("Update existing employee details").Border(BoxBorder.Rounded).BorderStyle("yellow").Header("[bold]Edit Employee[/]"));
        int id = InputHelper.ReadInt("Enter ID to edit");
        var emp = repo.FindById(id);
        if (emp is null)
        {
            AnsiConsole.MarkupLine("[red]Employee not found.[/]\n");
            return;
        }

        AnsiConsole.MarkupLine($"Editing [bold]{Markup.Escape(emp.GetDisplayName())}[/] (ID: {emp.Id})");
        AnsiConsole.MarkupLine("[grey50]Leave any field blank to keep current value.[/]");

        string firstName = InputHelper.ReadOptionalString($"First name ({emp.FirstName})");
        string lastName = InputHelper.ReadOptionalString($"Last name ({emp.LastName})");
        string ageStr = InputHelper.ReadOptionalString($"Age ({emp.Age})");
        string department = InputHelper.ReadOptionalString($"Department ({emp.Department})");
        string salaryStr = InputHelper.ReadOptionalString($"Salary ({emp.Salary})");

        string street = InputHelper.ReadOptionalString($"Street ({emp.Address.Street})");
        string city = InputHelper.ReadOptionalString($"City ({emp.Address.City})");
        string state = InputHelper.ReadOptionalString($"State ({emp.Address.State})");
        string zip = InputHelper.ReadOptionalString($"Zip ({emp.Address.Zip})");

        // Apply edits conditionally
        if (!string.IsNullOrWhiteSpace(firstName)) emp.FirstName = firstName;
        if (!string.IsNullOrWhiteSpace(lastName)) emp.LastName = lastName;
        if (int.TryParse(ageStr, out int age)) emp.Age = age;
        if (!string.IsNullOrWhiteSpace(department)) emp.Department = department;
        if (decimal.TryParse(salaryStr, out decimal salary)) emp.Salary = salary;

        var addr = emp.Address;
        if (!string.IsNullOrWhiteSpace(street)) addr.Street = street;
        if (!string.IsNullOrWhiteSpace(city)) addr.City = city;
        if (!string.IsNullOrWhiteSpace(state)) addr.State = state;
        if (!string.IsNullOrWhiteSpace(zip)) addr.Zip = zip;
        emp.Address = addr; // structs are value types

        AnsiConsole.MarkupLine("[green]Employee updated.[/]\n");
    }

    private static void ListEmployees(EmployeeRepository repo)
    {
        var panel = new Panel("Current employees").Border(BoxBorder.Rounded).BorderStyle("blue").Header("[bold]Employees[/]");
        AnsiConsole.Write(panel);
        if (repo.Employees.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No employees found.[/]\n");
            return;
        }

        var table = new Table().Border(TableBorder.Rounded).Title("[bold]Employees[/]");
        table.AddColumn("ID");
        table.AddColumn("Name");
        table.AddColumn("Dept");
        table.AddColumn("Salary");
        table.AddColumn("Address");
        table.AddColumn("Notes");

        foreach (var e in repo.Employees)
        {
            var seniority = e.Age >= 50 ? "[italic grey50](Senior)[/]" : string.Empty;
            table.AddRow(
                e.Id.ToString(),
                Markup.Escape(e.GetDisplayName()),
                Markup.Escape(e.Department),
                Markup.Escape(e.Salary.ToString("C")),
                Markup.Escape(e.Address.ToString()),
                seniority
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    private static void Save(EmployeeRepository repo)
    {
        try
        {
            repo.SaveToFile(DataFile);
            AnsiConsole.MarkupLine($"[green]Saved[/] [bold]{repo.Employees.Count}[/] employees to '[grey]{Markup.Escape(DataFile)}[/]'.\n");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Failed to save[/]: {Markup.Escape(ex.Message)}\n");
        }
    }

    private static void Load(EmployeeRepository repo)
    {
        try
        {
            repo.LoadFromFile(DataFile);
            AnsiConsole.MarkupLine($"[green]Loaded[/] [bold]{repo.Employees.Count}[/] employees from '[grey]{Markup.Escape(DataFile)}[/]'.\n");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Failed to load[/]: {Markup.Escape(ex.Message)}\n");
        }
    }
}
