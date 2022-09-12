using System;
using Simulador;

namespace Program1
{
    public class MemoryManager
    {
        public MemorySegment? head;
        public int memorySize;
        public bool show;
        public bool step;

        List<Process> disk;


        public MemoryManager(int memorySize)
        {
            this.memorySize = memorySize;
            this.disk = new List<Process>();
        }

        /// <summary>
        /// Adiciona um segmento à memória principal.
        /// </summary>
        /// <param name="size"></param>
        public void AddSegment(int size)
        {
            if (this.head == null) this.head = new MemorySegment(null, 0, size);
            else this.head.AddSegment(size);
            Feedback();
        }

        /// <summary>
        /// Adiciona um processo à memória principal.
        /// </summary>
        public void AddProcess(Process p, Algorithm algorithm)
        {
            try { this.head = algorithm.AddProcess(this.head, p); }
            catch (NoSegmentFitException)
            {
                Printer.YellowLn("Não há um segmento livre que caiba o processo. Adicionando segmento...");
                try { AddSegment(p.size); }
                catch (MemoryFullException)
                {
                    Printer.YellowLn("Memória cheia. Movendo processo para o disco (swapping)");
                    Swap();
                }


                AddProcess(p, algorithm);
            }
            Feedback();
        }

        /// <summary>
        /// Remove um processo da memória principal.
        /// </summary>
        public void RemoveProcess(int PID)
        {
            if (this.head == null) throw new Exception("Not initialized.");
            this.head.RemoveProcess(PID);
            Feedback();
        }

        /// <summary>
        /// Compacta a memória para reduzir fragmentação externa.
        /// </summary>
        public void Compact()
        {
            if (this.head == null) throw new Exception("Not initialized.");
            this.head.Compact();
            Feedback();
        }

        public void Swap()
        {
            Random rnd = new Random();
            List<Process> list = ProcessList();
            Process randomProcess = list[rnd.Next(list.Count)];

            RemoveProcess(randomProcess.PID);
            this.disk.Add(randomProcess);
        }

        public List<Process> ProcessList()
        {
            List<Process> list = new List<Process>();

            MemorySegment current = this.head;
            while (current != null)
            {
                if (current.State == 'P') list.Add(current.process);
            }

            return list;
        }

        public void Feedback()
        {
            if (this.show)
            {
                if (this.head == null) throw new Exception("Not initialized.");
                this.head.Print();
            }
            if (this.step) Console.ReadLine();
        }

    }
}