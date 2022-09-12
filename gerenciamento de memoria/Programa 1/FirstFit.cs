namespace Program1
{
    public class FirstFit : Algorithm
    {
        //Recebe inicialmente o head do MemoryManager e o processo p a ser alocado
        public MemorySegment AddProcess(MemorySegment current, Process p)
        {
            if (current.State == 'L' && current.length >= p.size)
            {
                current.process = p;
            }
            else if (current.next != null)
            {
                AddProcess(current.next, p);
            }
            else
            {
                throw new NoSegmentFitException();
            }
            return current;
        }
    }
}
