using Spectre.Console;

namespace EmployeeManagement.Utils;

public static class InputHelper
{
    public static string ReadString(string prompt)
    {
        var value = AnsiConsole.Prompt(
            new TextPrompt<string>($"[cyan]{Markup.Escape(prompt)}[/]:")
                .PromptStyle("white")
                .ValidationErrorMessage("[red]Value cannot be empty. Try again.[/]")
                .Validate(s => !string.IsNullOrWhiteSpace(s))
        );
        return value.Trim();
    }

    public static string ReadOptionalString(string prompt)
    {
        var value = AnsiConsole.Prompt(
            new TextPrompt<string>($"[cyan]{Markup.Escape(prompt)}[/]:")
                .PromptStyle("white")
                .AllowEmpty()
        );
        return value ?? string.Empty;
    }

    public static int ReadInt(string prompt)
    {
        var value = AnsiConsole.Prompt(
            new TextPrompt<int>($"[cyan]{Markup.Escape(prompt)}[/]:")
                .PromptStyle("white")
                .ValidationErrorMessage("[red]Please enter a valid integer.[/]")
        );
        return value;
    }

    public static decimal ReadDecimal(string prompt)
    {
        var value = AnsiConsole.Prompt(
            new TextPrompt<decimal>($"[cyan]{Markup.Escape(prompt)}[/]:")
                .PromptStyle("white")
                .ValidationErrorMessage("[red]Please enter a valid number.[/]")
        );
        return value;
    }
}
