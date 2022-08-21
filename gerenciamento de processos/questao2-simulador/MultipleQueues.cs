/// <summary>
/// Classe implementada por Luiz Miguel
/// </summary>
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
            this.queues[finalPriority].Add(process); // TODO: stack overflow
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
                    if (this.queues[l].Count > 0)
                    {
                        int c = 0;
                        Process process = this.queues[l][c];
                        this.queues[l].RemoveAt(c);
                        int difCPUTime = process.CPUTimeActive + timeExec - process.CPUTime;
                        if (difCPUTime < 0)
                            difCPUTime = 0;
                        timeExec -= difCPUTime;
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
                        Thread.Sleep(timeExec + this.quantPreempcao);
                        this.CheckTaskAging();
                        this.UpdateReadyQueue();
                        break;
                    }
                    timeExec *= 2;
                    if (l + 1 == this.queues.Count)
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
            this.readyQueue.RemoveRange(0, readyQueue.Count);
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
                    if (process.TaskAging >= this.quantPreempcao * 50)
                    {
                        this.queues[l].RemoveAt(c);
                        process.ResetTaskAging();
                        if (l - 1 >= 0)
                        {
                            this.queues[l - 1].Add(process);
                            Console.WriteLine(string.Join(" ", "[TaskAging]", process.Name, "movido para fila", l - 1));
                        }
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
            string saida = "";
            for (int l = 0; l < this.queues.Count; l++)
            {
                saida += string.Join("", "Queue ", l.ToString(), ":  ");
                for (int c = 0; c < this.queues[l].Count; c++)
                {
                    saida += string.Join("", this.queues[l][c].Name, " (", this.queues[l][c].CPUTimeActive.ToString(), " / ", this.queues[l][c].CPUTime.ToString(), ")  -->  ");
                }
                saida += "\n";
            }
            Console.Write(saida);
        }
    }
}