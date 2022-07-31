/// <summary>
/// Classe implementada por Rahmon Jorge
/// </summary>
namespace Simulador
{
    public class RoundRobin
    {
        private Queue<Process> ready;
        private Process? running;
        private Queue<Process> finished;
        private int quantum;
        private int processCount;
        private int totalWaitTime = 0;

        public RoundRobin(List<Process> processes, int quantum)
        {
            this.ready = new Queue<Process>(processes);
            this.quantum = quantum;
            this.finished = new Queue<Process>();
            this.processCount = processes.Count;
        }

        public void Run()
        {
            Console.WriteLine();
            for (int countdown = 4; countdown > 0; countdown--)
            {
                Console.WriteLine("Starting in " + countdown + "s...");
                Thread.Sleep(1000);
            }

            PrintProcesses();

            while (ready.Count > 0 || running != null)
            {
                // Ready -> Running
                if (running == null)
                {
                    running = ready.Dequeue();
                    Console.WriteLine("-> Moved process '" + running.Name + "' from ready queue to the CPU.");
                    PrintProcesses();
                }

                // Update process times
                running.TimeLeft -= quantum;    // Deduce Quantum from current running process

                if (running.TimeLeft < 0)
                {
                    running.Turnaround += Math.Abs(running.TimeLeft);
                    running.TimeLeft = 0;
                }
                else running.Turnaround += quantum;

                foreach (Process p in ready)
                {
                    p.Turnaround += quantum;    // Update turnaround
                    p.WaitingTime += quantum;   // Update waiting time
                    this.totalWaitTime += p.WaitingTime;
                }

                Console.WriteLine("-> Deducted " + quantum + "ms from current running process.");
                PrintProcesses();

                // Check if process is done
                if (running.IsFinished())
                {
                    finished.Enqueue(running);   // If it is finished, add to Finished queue
                    Console.WriteLine("-> Process finished. Moved to finished queue.");
                    running = null;
                }

                // Preempt
                else if (ready.Count > 0) // If there are process in the ready queue...
                {
                    ready.Enqueue(running); // Move current process to ready
                    Console.WriteLine("-> Process quantum ended. Moved to ready queue.");
                    running = null;
                }
            }
            Console.WriteLine("No processes ready nor running.\n");

            PrintStatistics();

            Console.WriteLine("\nRound-Robin simulation finished. Press to close.");

            Console.ReadLine();
        }

        public void PrintProcesses()
        {
            Console.WriteLine();
            Console.Write("READY: ");
            foreach (Process p in ready) Printer.PrintProcessStatusColor(p);
            Console.WriteLine();
            Console.Write("RUNNING: "); Printer.PrintProcessStatusColor(running);
            Console.WriteLine();
            Console.Write("FINISHED: ");
            foreach (Process p in finished) Printer.PrintProcessStatusColor(p);
            Console.Write("\n\nPress for next step...");
            Console.ReadLine();
            Console.WriteLine();
        }

        public void PrintStatistics()
        {
            List<string> output = new List<string>();

            foreach (Process p in finished) output.Add(p.Name + "'s turnaround time: " + p.Turnaround + "ms\n" + p.Name + "'s wait time: " + p.WaitingTime + "ms\n");
            output = output.OrderBy(q => q).ToList();

            Console.WriteLine("- STATISTICS -");
            foreach (string s in output) Console.WriteLine(s);
            Console.WriteLine("Average wait time for all processes: " + this.totalWaitTime / finished.Count + "ms");
        }
    }
}