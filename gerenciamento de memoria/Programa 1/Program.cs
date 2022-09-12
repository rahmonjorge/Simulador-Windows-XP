using System;
using System.Threading;
using Simulador;

public static class Program
{
    static void Main()
    {
        MemoryManager manager = new MemoryManager(1024);
        manager.show = true;
        manager.step = false;

        Console.WriteLine("\nAdicionando processos:");

        manager.AddProcess(new Process(PID: 1, size: 100));
        manager.AddProcess(new Process(2, 20));
        manager.AddProcess(new Process(3, 3));
        manager.AddProcess(new Process(4, 43));
        manager.AddProcess(new Process(5, 2));
        manager.AddProcess(new Process(6, 35));
        manager.AddProcess(new Process(7, 10));
        manager.AddProcess(new Process(8, 323));


        Console.WriteLine("\n Removendo processos:");

        manager.RemoveProcess(2);
        manager.RemoveProcess(3);
        manager.RemoveProcess(4);
        manager.RemoveProcess(5);
        manager.RemoveProcess(7);

        Console.WriteLine("\n Compactando:");

        //Console.WriteLine("Espaço não-segmentado: " + manager.memorySize - );

        manager.Compact();

        /* Console.WriteLine("\n First Fit:");
        manager.head = FirstFit.Run(manager.head, new Process(10, 35));
        manager.head.Print(); */

        /* Console.WriteLine("\n Best Fit:");
        manager.head = BestFit.Run(manager.head, new Process(10, 2), manager.idHead);
        manager.head.Print(); */











        /* Console.WriteLine("\n Worst Fit:");
        manager.head = BestFit.Run(manager.head, new Process(10, 2), manager.idHead);
        manager.head.Print(); */

    }
}