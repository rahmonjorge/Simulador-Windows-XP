class MemoryManager
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
    /// Adiciona um processo à memória principal.
    /// </summary>
    public void AddProcess(Process p)
    {
        if (this.head == null) this.head = new MemorySegment(0, p); // Se a cabeça da lista é vazia, crie um segmento na posição zero.
        else this.head.Add(p, memorySize); // Caso contrário, adicione no final da lista.
        Step();
    }

    /// <summary>
    /// Remove um processo da memória principal.
    /// </summary>
    /// <param name="PID"></param>
    public void RemoveProcess(int PID)
    {
        if (this.head == null) throw new Exception("Not initialized.");
        this.head.Remove(PID);
        Step();
    }

    public void Step()
    {
        if (this.show) this.head?.Print();
        if (this.step) Console.ReadLine();
    }
}