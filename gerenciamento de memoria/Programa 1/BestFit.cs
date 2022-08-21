public class BestFit{
    //Recebe inicialmente o head do MemoryManager e o processo p a ser alocado
    public void Run(MemorySegment current, Process p, int smaller){
        MemorySegment next = current.next;
        if(next.State == 'L' && next.length >= current.length){
            MemorySegment newSeg = new MemorySegment(p, next.begin, next.length);
            newSeg.next = next.next; //o novo segmento eh ligado ao proximo
            current.next = newSeg; //o espaco vazio eh desligado do proximo segmento
            //[TO-DO?] desalocar instancia de next?
        }
        else if(next.State == 'P'){
            this.Run(next, p);
        }
        else{
            //Iterador chega no fim da memoria e nao encontra espacos vazios
        }

    }
}