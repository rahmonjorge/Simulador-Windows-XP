using System;
/* 

Implementação do gerenciador de memória. A estrutura que representa a mesma é uma lista duplamente encadeada.

 */
public class MemoryManager
{
    public MemorySegment? head;
    public int idHead;
    public int memorySize;
    public bool show;
    public bool step;

    public static Random ranNum = new Random();

    public MemoryManager(int memorySize)
    {
        this.memorySize = memorySize;
        this.idHead = ranNum.Next(100,200);
    }


    /// <summary>
    /// Adiciona um segmento à memória principal.
    /// </summary>
    /// <param name="size"></param>
    public void AddSegment(int size)
    {
        if (this.head == null){
            this.head = new MemorySegment(null, 0, size, null);
        }
        else this.head.AddSegment(size);
        Step();
    }

    public void AddSegment(Process p, int size)
    {

    }

    /// <summary>
    /// Adiciona um processo à memória principal.
    /// </summary>
    public void AddProcess(Process p)
    {
        if (this.head == null){
            this.head = new MemorySegment(p, 0, p.size, null); // Se a cabeça da lista é vazia, crie um segmento na posição zero.
        }
        else{
            this.head.AddProcessSegment(p, memorySize); // Caso contrário, adicione no final da lista.
        }
        Step();
    }

    /// <summary>
    /// Remove um processo da memória principal.
    /// </summary>
    public void RemoveProcess(int PID)
    {
        if (this.head == null) throw new Exception("Not initialized.");
        this.head.RemoveProcess(PID);
        Step();
    }

    public void Compact()
    {
        if (this.head == null) throw new Exception("Not initialized.");
        this.head.Compact();
        Step();
    }

    public void Step()
    {
        if (this.show)
        {
            if (this.head == null) throw new Exception("Not initialized.");
            this.head.Print();
        }
        if (this.step) Console.ReadLine();
    }

}