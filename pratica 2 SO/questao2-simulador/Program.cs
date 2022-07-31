public class Process
{
    int id;
    string name;
    int priority;
    bool ioBound;
    bool cpuBound;
    int cpuTime; // Em ms
    int turnaround; // Em ms
    int taskAging; // Em ms
    int cpuTimeActive; //Em ms
    int waitingTime; //Em ms
    public Process(int id, string name, int priority, bool ioBound, bool cpuBound, int cpuTime)
    {
        this.id = id;
        this.name = name;
        this.priority = priority;
        this.ioBound = ioBound;
        this.cpuBound = cpuBound;
        this.cpuTime = cpuTime;
        this.turnaround = 0;
        this.taskAging = 0;
        this.cpuTimeActive = 0;
        this.waitingTime = 0;
    }

    public void preemption(int exec, int stop)
    {
        this.turnaround += exec + stop;
        this.taskAging += exec + stop;
        this.waitingTime += exec + stop;
    }

    public bool execution(int exec)
    {
        this.turnaround += exec;
        this.taskAging = 0;
        this.cpuTimeActive += exec;
        if (this.cpuTimeActive >= this.cpuTime)
        {
            this.cpuTimeActive = this.cpuTime;
            return true;
        }
        return false;
    }
    public string getBound()
    {
        if (this.cpuBound)
            return "CPU";
        return "IO";
    }
    public override string ToString()
    {
        return string.Join("", this.id, " - ", this.name, " ( Bound: ", this.getBound(), ", Priority: ", this.priority, ", CPU Time: ", this.cpuTime, ", CPU Active: ", this.cpuTimeActive, ", Turnaround: ", this.turnaround, ", TaskAging: ", this.taskAging, " )");
    }
    public int getPriority()
    {
        return this.priority;
    }
    public void setPriority(int p)
    {
        this.priority = p;
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
    public int getWaitingTime()
    {
        return this.waitingTime;
    }
    public int getCPUTimeActive()
    {
        return this.cpuTimeActive;
    }
    public int getTurnaround()
    {
        return this.turnaround;
    }
    public void addTurnaround(int e)
    {
        this.turnaround += e;
    }
    public int getTaskAging()
    {
        return this.taskAging;
    }
    public void zeroTaskAging()
    {
        this.taskAging = 0;
    }
}

public class MultipleQueues
{
    List<List<Process>> queues;
    List<Process> readyQueue;
    List<Process> finishedQueue;
    int quantPreempcao;

    public MultipleQueues(int quantPreemp)
    {
        this.quantPreempcao = quantPreemp;
        this.queues = new List<List<Process>>();
        this.readyQueue = new List<Process>();
        this.finishedQueue = new List<Process>();
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
        this.readyQueue.Add(process);
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
        this.run();
    }

    public void run()
    {
        for(bool condition = true; condition;)
        {
            bool executed = false;
            int timeExec = 1;
            this.printer();
            string saida = "Ready: ";
            foreach (Process item in this.readyQueue)
            {
                saida += item + "|||";
            }
            saida += "\nFinished: ";
            foreach (Process item in this.finishedQueue)
            {
                saida += item + "--> ";
            }
            Console.WriteLine(saida);
            for(int l = 0; l < this.queues.Count; l++)
            {
                for(int c = 0; c < this.queues[l].Count; c++)
                {
                    Process process = this.queues[l][c];
                    this.queues[l].RemoveAt(c);
                    bool finalize = process.execution(timeExec);
                    if (finalize)
                    {
                        this.finishedQueue.Add(process);
                        this.addPreemptionToProcess(timeExec);
                        Console.WriteLine("[Finalized] " + process.getName());
                    }
                    else
                    {
                        this.addPreemptionToProcess(timeExec);
                        process.addTurnaround(this.quantPreempcao);
                        int queue = l + 1;
                        if (queue < this.queues.Count)
                        {
                            this.queues[queue].Add(process);
                            Console.WriteLine("[Preemption] " + process.getName() + " movido para a fila" + string.Join("", queue));
                        }
                    }
                    executed = true;
                    break;
                }
                timeExec *= 2;
                
                if (executed)
                {
                    this.updateReadyQueue();
                    this.checkTaskAging();
                    break;
                }
                else if (l + 1 == this.queues.Count)
                    condition = false;
            }
            Console.WriteLine();
        }
        float totalWaiting = 0;
        foreach (Process item in this.finishedQueue)
        {
            totalWaiting += item.getWaitingTime();
        }
        float average = totalWaiting / finishedQueue.Count;
        Console.WriteLine(string.Join("", "\n\nTempo de Espera Médio: ", average));
    }
    
    public void updateReadyQueue()
    {
        this.readyQueue.RemoveRange(0, this.readyQueue.Count);
        foreach (List<Process> itemY in this.queues)
        {
            foreach (Process itemX in itemY)
                this.readyQueue.Add(itemX);
        }
    }
    public void checkTaskAging()
    {
        for(int l = 0; l < this.queues.Count; l++)
        {
            for(int c = 0; c < this.queues[l].Count; c++)
            {
                Process process = this.queues[l][c];
                if (process.getTaskAging() >= this.quantPreempcao * 100)
                {
                    this.queues[l].RemoveAt(c);
                    process.zeroTaskAging();
                    this.queues[l-1].Add(process);
                    Console.WriteLine(string.Join(" ", "[TaskAging]", process.getName(), "movido para fila", l-1));
                }
            }
        }
    }

    public void addPreemptionToProcess(int exec)
    {
        for(int l = 0; l < this.queues.Count; l++)
        {
            for(int c = 0; c < this.queues[l].Count; c++)
            {
                this.queues[l][c].preemption(exec, this.quantPreempcao);
            }
        }
    }

    public void printer()
    {
        for(int l = 0; l < this.queues.Count; l++)
        {
            Console.Write(string.Join("", "Queue ", l.ToString(), ":  "));
            for(int c = 0; c < this.queues[l].Count; c++)
            {
                string saida = string.Join("", this.queues[l][c].getName(), " (", this.queues[l][c].getCPUTimeActive().ToString(), " / ", this.queues[l][c].getCPUTime().ToString(), ")  -->  ");
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
        List<Process> listProcess = new List<Process>();
        Console.WriteLine("Crie sua lista de Processos a seguir:");
        Console.WriteLine("Quantos Processos deseja criar?");
        int qtdProcess = Int32.Parse(Console.ReadLine());

        for(int id = 1; id <= qtdProcess; id++)
        {
            Console.WriteLine("Qual o Nome do Processo?");
            string name = Console.ReadLine();
            Console.WriteLine("Qual a Prioridade do Processo?");
            int priority = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Ele é CPU ou IO Bound?");
            string boundStr = Console.ReadLine();
            Console.WriteLine("Qual o tempo de CPU do Processo?");
            int cpuTime = Int32.Parse(Console.ReadLine());
            bool cpuBound = boundStr == "CPU";
            bool ioBound = boundStr == "IO";
            
            Process process = new Process(id, name, priority, ioBound, cpuBound, cpuTime);
            listProcess.Add(process);
            Console.WriteLine(process);
            Console.WriteLine();
        }
        Console.WriteLine("Qual o tempo do quantum da preempção (ms)?");
        int timePreemp = Int32.Parse(Console.ReadLine());
        Console.WriteLine("Qual Algoritmo deseja utilizar?\n1 - MultipleQueues\n2 - ?????????\n3 - ???????????");
        int algoChoice = Int32.Parse(Console.ReadLine());
        for(bool condition = true; condition;)
        {
            switch (algoChoice)
            {
                case 1:
                    MultipleQueues algo = new MultipleQueues(timePreemp);
                    algo.addListProcess(listProcess);
                    algo.run();
                    condition = false;
                    break;
                case 2:
                    condition = false;
                    break;
                case 3:
                    condition = false;
                    break;
                default:
                    Console.WriteLine("Escolha de novo...");
                    break;
            }
        }
    }
}

