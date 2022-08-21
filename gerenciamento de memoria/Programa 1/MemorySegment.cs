using System;
using Simulador;

public class MemorySegment
{
    public Process? process; // Processo contido no segmento.
    public int begin; // Endereço onde começa o segmento
    public int length; // Comprimento do segmento
    public MemorySegment? next = null; // Aponta para o próximo segmento

    public char State // Indica se há ou não um processo no segmento.
    {
        get { return this.process == null ? 'L' : 'P'; }
    }

    public MemorySegment(Process? p, int begin, int length)
    {
        this.process = p;
        this.begin = begin;
        this.length = length;
    }

    // Adiciona um novo segmento contendo um processo ao final da lista.
    public void AddProcessSegment(Process p, int limit)
    {
        if (this.next == null)
        {
            MemorySegment newSegment = new MemorySegment(p, this.End() + 1, p.size);

            if (newSegment.End() >= limit) throw new Exception("New segment doesn't fit in memory.");
            else this.next = newSegment;
        }
        else next.AddProcessSegment(p, limit);
    }

    // Libera um segmento de memória que está ocupado pelo processo especificado.
    public void RemoveProcess(int PID)
    {
        if (this.process != null && this.process.PID == PID) this.process = null; // Se este processo é igual ao que está sendo procurado, remover.
        else if (this.next != null) this.next.RemoveProcess(PID); // Caso contrário, verificar o próximo.
        else throw new Exception("No process found with the PID: " + PID);
    }

    // Adiciona um novo segmento ao final da lista.
    public void AddSegment(int size)
    {
        if (this.next == null) this.next = new MemorySegment(null, this.End() + 1, size);
        else next.AddSegment(size);
    }

    // Move todos os espaços vazios encontrados para o final da lista.
    public void Compact()
    {
        while (this.IsFragmented())
        {
            this.MoveEmpty();
        }
    }

    // Verifica se há necessidade de compactação.
    public bool IsFragmented()
    {
        if (this.next != null)
        {
            if (this.State == 'L' && this.next.State == 'P')
            {
                return true;
            }
            else
            {
                return this.next.IsFragmented();
            }
        }
        return false;
    }

    // Move o primeiro espaço vazio encontrado para o final da lista
    private void MoveEmpty()
    {
        MemorySegment? freeSpace = null;

        if (this.next != null)
        {
            if (this.next.State == 'L') // Se o próximo segmento está livre
            {
                freeSpace = this.next; // Salvar o proximo numa variável
                this.next = freeSpace.next; // Unir a ponta do próximo com o meu próximo (remover o espaço livre)
            }
            else
                this.next.MoveEmpty();
        }

        if (freeSpace != null)
        {
            freeSpace.next = null; // Cortar a cauda do freespace, pois ele ira pro final da lista
            this.AddSegment(freeSpace.length); // Adicionar o espaço vazio no final da lista
        }
    }

    public void Print()
    {
        if (this.State == 'L') Printer.Green($"[{this.State}/{this.begin}/{this.length}] -> ");
        else Printer.Blue($"[{this.State}/{this.begin}/{this.length}] -> ");
        if (this.next != null) this.next.Print();
        else Printer.Red("null\n");
    }

    private int End()
    {
        return this.begin + this.length - 1; // Se começa em 0, e o tamanho é 5, a última posição é 4.
    }
}
