using System;
using System.Threading;
using Simulador;

namespace Program1
{
    public static class Program
    {
        public const int MEMSIZE = 100;

        static void Main()
        {
            MemoryManager manager = new MemoryManager(MEMSIZE);
            manager.show = true;
            manager.step = false;

            Console.WriteLine("\nCriando Segmentos:");

            manager.AddSegment(1);
            manager.AddSegment(2);
            manager.AddSegment(3);
            manager.AddSegment(4);
            manager.AddSegment(5);
            manager.AddSegment(6);
            manager.AddSegment(7);
            manager.AddSegment(8);
            manager.AddSegment(9);
            manager.AddSegment(10);

            Console.WriteLine("\n Adicionando Processos com First Fit:");

            Algorithm ff = new FirstFit();

            manager.AddProcess(new Process(1, 4), ff);
            manager.AddProcess(new Process(1, 12), ff);
            manager.AddProcess(new Process(1, 20), ff);
            manager.AddProcess(new Process(1, 19), ff);

            /*
                manager.AddProcess(new Process(1, 1));
                manager.AddProcess(new Process(2, 2));
                manager.AddProcess(new Process(3, 3));
                manager.AddProcess(new Process(4, 4));
                manager.AddProcess(new Process(5, 5));
                manager.AddProcess(new Process(6, 6));
                manager.AddProcess(new Process(7, 7));
                manager.AddProcess(new Process(8, 8));




                manager.RemoveProcess(2);
                manager.RemoveProcess(3);
                manager.RemoveProcess(4);
                manager.RemoveProcess(5);
                manager.RemoveProcess(7);

                Console.WriteLine("\n Compactando:");

                //Console.WriteLine("Espaço não-segmentado: " + manager.memorySize - );

                manager.Compact();

                Console.WriteLine("\n First Fit:");
                //manager.head = FirstFit.Run(manager.head, new Process(10, 7));
                manager.head.Print();
        */
        }
    }
}