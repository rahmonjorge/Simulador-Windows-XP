namespace Simulador
{
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

        public void AddProcess(Process process)
        {
            int finalPriority = process.Priority;
            if (!process.CPUBound)
            {
                if (finalPriority >= 7)
                    finalPriority = 9;
                else
                    finalPriority += 2;
            }
            this.queues[finalPriority].Add(process);
            this.readyQueue.Add(process);
        }

        public void AddProcessList(List<Process> listProcess)
        {
            for (int i = 0; i < listProcess.Count; i++)
            {
                this.AddProcess(listProcess[i]);
            }
        }

        public void Start()
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
            this.AddProcessList(listProcess);
            this.Run();
        }

        public void Run()
        {
            for (bool condition = true; condition;)
            {
                bool executed = false;
                int timeExec = 1;
                this.Printer();
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
                for (int l = 0; l < this.queues.Count; l++)
                {
                    for (int c = 0; c < this.queues[l].Count; c++)
                    {
                        Process process = this.queues[l][c];
                        this.queues[l].RemoveAt(c);
                        bool finalize = process.Execution(timeExec);
                        if (finalize)
                        {
                            this.finishedQueue.Add(process);
                            this.AddPreemptionToProcess(timeExec);
                            Console.WriteLine("[Finalized] " + process.Name);
                        }
                        else
                        {
                            this.AddPreemptionToProcess(timeExec);
                            process.AddTurnaround(this.quantPreempcao);
                            int queue = l + 1;
                            if (queue < this.queues.Count)
                            {
                                this.queues[queue].Add(process);
                                Console.WriteLine("[Preemption] " + process.Name + " movido para a fila" + string.Join("", queue));
                            }
                        }
                        executed = true;
                        break;
                    }
                    timeExec *= 2;

                    if (executed)
                    {
                        this.UpdateReadyQueue();
                        this.CheckTaskAging();
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
                totalWaiting += item.WaitingTime;
            }
            float average = totalWaiting / finishedQueue.Count;
            Console.WriteLine(string.Join("", "\n\nTempo de Espera Médio: ", average));
        }

        public void UpdateReadyQueue()
        {
            this.readyQueue.RemoveRange(0, this.readyQueue.Count);
            foreach (List<Process> itemY in this.queues)
            {
                foreach (Process itemX in itemY)
                    this.readyQueue.Add(itemX);
            }
        }
        public void CheckTaskAging()
        {
            for (int l = 0; l < this.queues.Count; l++)
            {
                for (int c = 0; c < this.queues[l].Count; c++)
                {
                    Process process = this.queues[l][c];
                    if (process.TaskAging >= this.quantPreempcao * 100)
                    {
                        this.queues[l].RemoveAt(c);
                        process.ResetTaskAging();
                        this.queues[l - 1].Add(process);
                        Console.WriteLine(string.Join(" ", "[TaskAging]", process.Name, "movido para fila", l - 1));
                    }
                }
            }
        }

        public void AddPreemptionToProcess(int exec)
        {
            for (int l = 0; l < this.queues.Count; l++)
            {
                for (int c = 0; c < this.queues[l].Count; c++)
                {
                    this.queues[l][c].Preemption(exec, this.quantPreempcao);
                }
            }
        }

        public void Printer()
        {
            for (int l = 0; l < this.queues.Count; l++)
            {
                Console.Write(string.Join("", "Queue ", l.ToString(), ":  "));
                for (int c = 0; c < this.queues[l].Count; c++)
                {
                    string saida = string.Join("", this.queues[l][c].Name, " (", this.queues[l][c].CPUTimeActive.ToString(), " / ", this.queues[l][c].CPUTime.ToString(), ")  -->  ");
                    Console.Write(saida);
                }
                Console.WriteLine();
            }
        }
    }
}