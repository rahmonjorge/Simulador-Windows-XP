namespace Simulador
{
    public class Process
    {
        #region Fields
        public string Name { get { return this.name; } set { } }
        public int Priority { get { return this.priority; } set { } }
        public bool CPUBound { get { return this.CPUBound; } set { } }
        public int CPUTime { get { return this.cpuTime; } set { } }
        public int TaskAging { get { return this.taskAging; } set { } }
        public int CPUTimeActive { get { return this.cpuTimeActive; } set { } }
        public int WaitingTime { get { return this.waitingTime; } set { } }
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
            return string.Join("", this.id, " - ", this.name, " ( Bound: ", this.GetBound(), ", Priority: ", this.priority, ", CPU Time: ", this.cpuTime, ", CPU Active: ", this.cpuTimeActive, ", Turnaround: ", this.turnaround, ", TaskAging: ", this.taskAging, " )");
        }

        public void AddTurnaround(int e)
        {
            this.turnaround += e;
        }

        public void ResetTaskAging()
        {
            this.taskAging = 0;
        }
    }

}