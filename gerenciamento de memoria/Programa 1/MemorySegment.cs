using Simulador;

class MemorySegment
{
    public Process? process; // Processo contido no segmento.
    public int begin; // Endereço onde começa o segmento
    public int length; // Comprimento do segmento
    public MemorySegment? next = null; // Aponta para o próximo segmento

    public char State // Indica se há ou não um processo no segmento.
    {
        get { return this.process == null ? 'L' : 'P'; }
    }

    public MemorySegment(int begin, Process p)
    {
        this.process = p;
        this.begin = begin;
        this.length = p.size;
    }

    public void Add(Process p, int limit)
    {
        if (this.next == null)
        {
            MemorySegment newSegment = new MemorySegment(this.End() + 1, p);

            if (newSegment.End() >= limit) throw new Exception("New segment doesn't fit in memory.");
            else this.next = newSegment;
        }
        else next.Add(p, limit);
    }

    public void Remove(int PID)
    {
        if (this.process != null && this.process.PID == PID) this.process = null; // Se este processo é igual ao que está sendo procurado, remover.
        else if (this.next != null) this.next.Remove(PID); // Caso contrário, verificar o próximo.
        else throw new KeyNotFoundException("No process found with the PID: " + PID);
    }

    private int End()
    {
        return this.begin + this.length - 1; // Se começa em 0, e o tamanho é 5, a última posição é 4.
    }

    public void Print()
    {
        if (this.State == 'L') Printer.Green($"[{this.State}/{this.begin}/{this.length}] -> ");
        else Printer.Blue($"[{this.State}/{this.begin}/{this.length}] -> ");
        if (this.next != null) this.next.Print();
        else Printer.Red("null\n");
    }
}
