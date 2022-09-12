namespace Program1
{
    public static class BestFit
    {
        //Recebe inicialmente o head do MemoryManager, o processo p a ser alocado e um MemorySegment nulo que sera o menor a cada iteracao
        public static void Run(MemorySegment current, Process p, MemorySegment smaller)
        {
            MemorySegment next = current.next;
            if (smaller == null)
            { //primeira interacao
                smaller = new MemorySegment(null, -1, -1);
                smaller.next = current; //por enquanto, o menor eh o primeiro da lista (o head)
            }
            if (next.State == 'L')
            {
                next.process = p;
            }
            else if (next.State == 'P')
            {
                //Run(next, p);
            }
            else
            {
                //Iterador chega no fim da memoria e nao encontra espacos vazios
            }

        }
    }
}