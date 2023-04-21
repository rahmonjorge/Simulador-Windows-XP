namespace HTools;

/// <summary>
/// Class that handles user input for console applications.
/// </summary>
static class Input
{
    public static readonly string STANDARD_PREFIX = "->";

    public static string[] GetArgs(string prefix)
    {
        string? input = GetUserInput(prefix);
        if (input == null) return Array.Empty<string>();
        else return input.Split(' ');
    }

    public static string? GetUserInput(string prefix)
    {
        Printer.Yellow(prefix + " ");
        string? input = Console.ReadLine();
        return input?.ToLower();
    }

    public static string? GetUserInput() => GetUserInput(STANDARD_PREFIX);

    public static string[] GetArgs() => GetArgs(STANDARD_PREFIX);
}