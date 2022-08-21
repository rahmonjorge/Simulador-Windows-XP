public class MemoryManager
{
    public MemorySegment? head;
    public int memorySize;
    public bool show;
    public bool step;

    public MemoryManager(int memorySize)
    {
        this.memorySize = memorySize;
    }


    /// <summary>
    /// Adiciona um segmento à memória principal.
    /// </summary>
    /// <param name="size"></param>
    public void AddSegment(int size)
    {
        if (this.head == null) this.head = new MemorySegment(null, 0, size);
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
        if (this.head == null) this.head = new MemorySegment(p, 0, p.size); // Se a cabeça da lista é vazia, crie um segmento na posição zero.
        else this.head.AddProcessSegment(p, memorySize); // Caso contrário, adicione no final da lista.
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