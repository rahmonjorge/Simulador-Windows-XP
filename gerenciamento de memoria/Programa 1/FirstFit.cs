public static class FirstFit
{
    /* 
    <FUNCIONAMENTO>A função recebe o nó pai/cabeça da lista (head) e o processo a ser alocado
        <> O melhor encaixe é definido pelo primeiro segmento em que couber o processo.
     */
    public static MemorySegment Run(MemorySegment current, Process p)
    {
        if (current.State == 'L' && current.length >= p.size)
        {
            current.process = p;
        }
        else
        {
            Run(current.next, p);
        }
        return current;
    }
}