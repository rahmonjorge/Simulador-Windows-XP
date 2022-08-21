using System;
using System.Threading;
using Simulador;

static class Program
{
    static void Main()
    {
        MemoryManager manager = new MemoryManager(100);
        manager.show = true;
        manager.step = false;

        Console.WriteLine("\nAdicionando processos:");

        manager.AddProcess(new Process(1, 1));
        manager.AddProcess(new Process(2, 2));
        manager.AddProcess(new Process(3, 3));
        manager.AddProcess(new Process(4, 4));
        manager.AddProcess(new Process(5, 5));
        manager.AddProcess(new Process(6, 6));
        manager.AddProcess(new Process(7, 7));
        manager.AddProcess(new Process(8, 8));


        Console.WriteLine("\n Removendo processos:");

        manager.RemoveProcess(2);
        manager.RemoveProcess(3);
        manager.RemoveProcess(4);
        manager.RemoveProcess(5);
        manager.RemoveProcess(8);

        Console.WriteLine("\n Compactando:");

        //Console.WriteLine("Espaço não-segmentado: " + manager.memorySize - );

        manager.Compact();
    }
}