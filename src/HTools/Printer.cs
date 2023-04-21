namespace HTools;

/// <summary>
/// Powerful Printer for the Console that supports color output.
/// </summary>
public static class Printer
{
    public static void PrintColor(string msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(msg);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void PrintColorLn(string msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(msg);
        Console.ForegroundColor = ConsoleColor.White;
    }


    // Prints text in rainbow colors... what? why not?
    public static void PrintRainbow(string msg)
    {
        try
        {
            for (int i = 0; i < msg.Length; i += 6)
            {
                DarkRed(msg[i].ToString());
                DarkYellow(msg[i + 1].ToString());
                DarkGreen(msg[i + 2].ToString());
                DarkBlue(msg[i + 3].ToString());
                DarkMagenta(msg[i + 4].ToString());
                Magenta(msg[i + 5].ToString());
            }
        }
        catch (IndexOutOfRangeException)
        {
            PrintLn("");
        }
    }

    public static void PrintRainbowLn(string msg)
    {
        PrintRainbow(msg + "\n");
    }

    public static void PrintRainbowLnList(List<string> msg)
    {
        try
        {
            for (int i = 0; i < msg.Count; i += 6)
            {
                DarkRed(msg[i]);
                DarkYellow(msg[i + 1]);
                DarkGreen(msg[i + 2]);
                DarkBlue(msg[i + 3]);
                DarkMagenta(msg[i + 4]);
                Magenta(msg[i + 5]);
            }
        }
        catch (IndexOutOfRangeException)
        {
            PrintLn("");
        }
        Console.WriteLine();
    }

    public static void BlueLn(string msg) => PrintColorLn(msg, ConsoleColor.Blue);

    public static void DarkBlueLn(string msg) => PrintColorLn(msg, ConsoleColor.DarkBlue);

    public static void DarkRedLn(string msg) => PrintColorLn(msg, ConsoleColor.DarkRed);

    public static void DarkYellowLn(string msg) => PrintColorLn(msg, ConsoleColor.DarkYellow);

    public static void GreenLn(string msg) => PrintColorLn(msg, ConsoleColor.Green);

    public static void MagentaLn(string msg) => PrintColorLn(msg, ConsoleColor.Magenta);

    public static void RedLn(string msg) => PrintColorLn(msg, ConsoleColor.Red);

    public static void YellowLn(string msg) => PrintColorLn(msg, ConsoleColor.Yellow);

    public static void Blue(string msg) => PrintColor(msg, ConsoleColor.Blue);

    public static void DarkBlue(string msg) => PrintColor(msg, ConsoleColor.DarkBlue);

    public static void DarkGreen(string msg) => PrintColor(msg, ConsoleColor.DarkGreen);

    public static void DarkMagenta(string msg) => PrintColor(msg, ConsoleColor.DarkMagenta);

    public static void DarkRed(string msg) => PrintColor(msg, ConsoleColor.DarkRed);

    public static void DarkYellow(string msg) => PrintColor(msg, ConsoleColor.DarkYellow);

    public static void Green(string msg) => PrintColor(msg, ConsoleColor.Green);

    public static void Magenta(string msg) => PrintColor(msg, ConsoleColor.DarkMagenta);

    public static void Red(string msg) => PrintColor(msg, ConsoleColor.Red);

    public static void Yellow(string msg) => PrintColor(msg, ConsoleColor.Yellow);

    public static void SystemYellow(string msg) => YellowLn("\n" + msg + "\n");

    public static void SystemWhite(string msg) => PrintLn("\n" + msg + "\n");

    public static void Print(string msg)
    {
        Console.Write(msg);
    }

    public static void PrintLn()
    {
        Console.WriteLine();
    }

    public static void PrintLn(string msg)
    {
        Console.WriteLine(msg);
    }

}