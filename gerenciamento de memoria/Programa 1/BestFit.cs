using System;
/* 
Classe com função que implementa o algoritmo de Best Fit
 */
 public static class BestFit
{
    /* 
    <FUNCIONAMENTO>A função recebe o nó pai/cabeça da lista (head), o processo a ser alocado e o id do pai
        <> O melhor encaixe é definido com o uso de variáveis delta que calculam a menor variação entre o
        tamanho do segmento atual e o tamanho do processo a ser alocado.
        <> Sendo uma lista encadeada, o algoritmo itera duas vezes:
            - Na primeira da direita para a esquerda, buscando o melhor encaixe e anotando o id deste
            respectivo segmento;
            - Na segunda da esquerda para a direita (retornando para o nó pai), encaixa o processo no
            melhor segmento encontrado na primeira iteração.
     */
    public static MemorySegment Run(MemorySegment head, Process p, int idHead){
        int idCounter = idHead;
        MemorySegment current = head;
        int deltaLen;
        int smallerDeltaLen = 2147483647;
        int smallerLenId = idCounter;

        while(true){ // -->
            current.id = idCounter;
            deltaLen = current.length - p.size;
            if(deltaLen >= 0 && deltaLen < smallerDeltaLen && current.State == 'L'){
                smallerDeltaLen = deltaLen;
                smallerLenId = current.id;
            }
            /* if(current.State == 'L' && current.length >= p.size && current.length <= smallerDeltaLen){
                smallerLenId = current.id;
            } */
            idCounter += 1;
            if(current.next != null){   
                current = current.next;
            }
            else{
                break;
            }
        }

        while(current.prev != null){ // <--
            if(current.id == smallerLenId){
                current.process = p;
            }
            current = current.prev;
        }
        return current;
    }
}