namespace HTools;

/// <summary>
/// Class that builds menus for being used in the console output.
/// </summary>
public class Menu
{
    public string Header { get; set; } = "";
    public List<string> Items { get; set; }
    public string Capsule { get; set; } = "[]";
    public bool exit = false;

    public Menu(List<string> items)
    {
        Items = items;
        Enumerate();
    }

    public void Enumerate()
    {
        for (int i = 1; i <= Items.Count; i++)
        {
            Items[i - 1] = Capsule[0].ToString() + i + Capsule[1].ToString() + " " + Items[i - 1];
        }
    }

    public string? Run()
    {
        if (Header != null && Header.Length > 0) Printer.SystemYellow(Header);
        foreach (string item in Items) Console.WriteLine(item);

        return Input.GetUserInput(">> ");
    }
}