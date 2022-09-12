using HTools;

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
            Console.WriteLine("Round Robin Simulation started.");

            PrintProcesses();

            while (ready.Count > 0 || running != null)
            {
                // Ready -> Running
                if (running == null)
                {
                    running = ready.Dequeue();
                    Console.WriteLine("-> Moved '" + running.Name + "' from ready queue to the CPU.");
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
            PrintProcesses();
            Console.WriteLine("No processes ready nor running.\n");

            PrintStatistics();

            Console.WriteLine("\nRound-Robin simulation finished. Press to close.");

            Console.ReadLine();
        }

        public void PrintProcesses()
        {
            Console.WriteLine();
            Console.Write("READY: ");
            foreach (Process p in ready) PrintProcessStatusColor(p);
            Console.WriteLine();
            Console.Write("RUNNING: "); PrintProcessStatusColor(running);
            Console.WriteLine();
            Console.Write("FINISHED: ");
            foreach (Process p in finished) PrintProcessStatusColor(p);
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

        public static void PrintProcessStatusColor(Process? p)
        {
            if (p == null) return;
            string s = "(" + p.Name + ":" + p.TimeLeft + "ms)";
            ConsoleColor color = ConsoleColor.White;
            switch (p.ID)
            {
                case 1:
                    color = ConsoleColor.DarkRed;
                    break;
                case 2:
                    color = ConsoleColor.DarkYellow;
                    break;
                case 3:
                    color = ConsoleColor.DarkGreen;
                    break;
                case 4:
                    color = ConsoleColor.DarkBlue;
                    break;
                case 5:
                    color = ConsoleColor.DarkMagenta;
                    break;
                case 6:
                    color = ConsoleColor.Magenta;
                    break;
                default:
                    break;
            }
            Printer.PrintColor(s, color);
        }
    }
}