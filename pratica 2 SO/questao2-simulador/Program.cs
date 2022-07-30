class Process
{
    int id;
    string name;
    int priority;
    bool ioBound;
    bool cpuBound;
    int cpuTime; // Em ms

    public Process(int id, string name, int priority, bool ioBound, bool cpuBound, int cpuTime)
    {
        this.id = id;
        this.name = name;
        this.priority = priority;
        this.ioBound = ioBound;
        this.cpuBound = cpuBound;
        this.cpuTime = cpuTime; // Em ms
    }
}

public class Program
{
    static void Main()
    {
        Console.WriteLine("Hello, World!");

        List<Process> processes = new List<Process>();
    }
}
