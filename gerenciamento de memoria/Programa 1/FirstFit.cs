public static class FirstFit
{
    //Recebe inicialmente o head do MemoryManager e o processo p a ser alocado
    public static MemorySegment Run(MemorySegment current, Process p){
        if(current.State == 'L' && current.length >= p.size){
            current.process = p;
        }
        else{
            Run(current.next, p);
        }
        return current;
    }
}