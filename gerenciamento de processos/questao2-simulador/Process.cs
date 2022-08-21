/// <summary>
/// Classe implementada por Luiz Miguel e Rahmon Jorge
/// </summary>
namespace Simulador
{
    public class Process
    {
        #region Fields
        public int ID { get { return this.id; } set { this.id = value; } }
        public string Name { get { return this.name; } set { this.name = value; } }
        public int Priority { get { return this.priority; } set { this.priority = value; } }
        public bool CPUBound { get { return this.cpuBound; } set { this.cpuBound = value; } }
        public int CPUTime { get { return this.cpuTime; } set { this.cpuTime = value; } }
        public int Turnaround { get { return this.turnaround; } set { this.turnaround = value; } }
        public int TaskAging { get { return this.taskAging; } set { this.taskAging = value; } }
        public int CPUTimeActive { get { return this.cpuTimeActive; } set { this.cpuTimeActive = value; } }
        public int WaitingTime { get { return this.waitingTime; } set { this.waitingTime = value; } }
        public int TimeLeft { get { return this.timeLeft; } set { this.timeLeft = value; } }
        #endregion

        #region Properties
        private int id;
        private string name;
        private int priority;
        private bool ioBound;
        private bool cpuBound;
        private int cpuTime; // Em ms
        private int turnaround; // Em ms
        private int taskAging; // Em ms
        private int cpuTimeActive; //Em ms
        private int waitingTime; //Em ms
        private int timeLeft; //em ms
        #endregion

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
            this.timeLeft = cpuTime;
        }

        public void Preemption(int exec, int stop)
        {
            this.turnaround += exec + stop;
            this.taskAging += exec + stop;
            this.waitingTime += exec + stop;
        }

        public bool Execution(int exec)
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

        public string GetBound()
        {
            if (this.cpuBound)
                return "CPU";
            return "IO";
        }

        public override string ToString()
        {
            return "(PID:" + this.id + ", Name: " + this.name + ", Bound: " + this.GetBound() + ", Priority: " + this.priority + ", CPU Time: " + this.cpuTime + ", CPU Active: " + this.cpuTimeActive + ", Turnaround: " + this.turnaround + ", TaskAging: " + this.taskAging + ")";
        }

        public void AddTurnaround(int e)
        {
            this.turnaround += e;
        }

        public void ResetTaskAging()
        {
            this.taskAging = 0;
        }

        public bool IsFinished()
        {
            return this.timeLeft <= 0;
        }

        public string Status()
        {
            return "(" + this.Name + ":" + this.TimeLeft + "ms)";
        }
    }

}