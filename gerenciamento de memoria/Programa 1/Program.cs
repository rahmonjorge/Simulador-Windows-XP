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
        manager.AddProcess(new Process(2, 1));
        manager.AddProcess(new Process(3, 1));
        manager.AddProcess(new Process(4, 1));
        manager.AddProcess(new Process(5, 1));
        manager.AddProcess(new Process(6, 1));
        manager.AddProcess(new Process(7, 1));
        manager.AddProcess(new Process(8, 1));


        Console.WriteLine("\n Removendo processos:");

        manager.RemoveProcess(2);
        manager.RemoveProcess(5);
        manager.RemoveProcess(8);
    }
}