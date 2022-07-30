public class Process
{
    int id;
    string name;
    int priority;
    bool ioBound;
    bool cpuBound;
    int cpuTime; // Em ms
    int turnaround; // Em ms

    public Process(int id, string name, int priority, bool ioBound, bool cpuBound, int cpuTime)
    {
        this.id = id;
        this.name = name;
        this.priority = priority;
        this.ioBound = ioBound;
        this.cpuBound = cpuBound;
        this.cpuTime = cpuTime; // Em ms
        this.turnaround = 0;
    }

    public void preemption(int exec, int stop)
    {
        this.turnaround += exec + stop;
    }

    public void finalize(int exec)
    {
        this.turnaround += exec;
    }

    public int getPriority()
    {
        return this.priority;
    }
    public bool getCPUBound()
    {
        return this.cpuBound;
    }
    public bool getIOBound()
    {
        return this.ioBound;
    }

    public int getId()
    {
        return this.id;
    }
    public string getName()
    {
        return this.name;
    }
    public int getCPUTime()
    {
        return this.cpuTime;
    }

    public int getTurnaround()
    {
        return this.turnaround;
    }
}

public class MultipleQueues
{
    List<List<Process>> queues;
    int quantPreempcao;

    public MultipleQueues(int quantPreemp)
    {
        this.quantPreempcao = quantPreemp;
        this.queues = new List<List<Process>>();
        for (int i = 0; i < 10; i++)
        {
            this.queues.Add(new List<Process>());
        }
    }

    public void addProcess(Process process)
    {
        int finalPriority = process.getPriority();
        if (!process.getCPUBound())
        {
            if (finalPriority >= 7)
                finalPriority = 9;
            else
                finalPriority += 2;
        }
        this.queues[finalPriority].Add(process);
    }

    public void addListProcess(List<Process> listProcess)
    {
        for (int i = 0; i < listProcess.Count; i++)
        {
            this.addProcess(listProcess[i]);
        }
    }

    public void start()
    {   
        List<Process> listProcess = new List<Process>();
        Random rnd = new Random();
        for (int i = 0; i < 15; i++)
        {
            int priority = rnd.Next(5);
            string nome = string.Join("", "Process ", i.ToString());
            bool cpuBound = (rnd.Next(2)) == 0;
            bool ioBound = !cpuBound;
            int cpuTime = rnd.Next(513) + 1;
            listProcess.Add(new Process(i, nome, priority, cpuBound, ioBound, cpuTime));
        }
        this.addListProcess(listProcess);
        this.printer();
    }

    public void printer()
    {
        for(int l = 0; l < this.queues.Count; l++)
        {
            Console.Write(string.Join("", "Queue ", l.ToString(), ":  "));
            for(int c = 0; c < this.queues[l].Count; c++)
            {
                string saida = string.Join("", this.queues[l][c].getName(), " (", this.queues[l][c].getCPUTime().ToString(), ")  -->  ");
                Console.Write(saida);
            }
            Console.WriteLine();
        }
    }
}



public class Program
{
    static void Main()
    {
        MultipleQueues algo = new MultipleQueues(10);
        algo.start();
    }
}

