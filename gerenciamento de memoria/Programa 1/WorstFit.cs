/* using System;

public static class WorstFit
{
    public static MemorySegment Run(MemorySegment head, Process p, int idHead){
        int idCounter = idHead;
        MemorySegment current = head;
        int deltaLen;
        int greaterDeltaLen = -1;
        int greaterLenId = idCounter;

        while(true){ // -->
            current.id = idCounter;
            deltaLen = current.length - p.size;
            if(deltaLen >= 0 && deltaLen > greaterDeltaLen && current.State == 'L'){
                greaterDeltaLen = deltaLen;
                greaterLenId = current.id;
            }
            idCounter += 1;
            if(current.next != null){   
                current = current.next;
            }
            else{
                break;
            }
        }

        while(current.prev != null){ // <--
            if(current.id == greaterLenId){
                current.process = p;
            }
            current = current.prev;
        }
        return current;
    }
} */