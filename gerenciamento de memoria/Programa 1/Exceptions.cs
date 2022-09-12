namespace Program1
{
    /// <summary>
    /// É levantada quando não foi encontrado um segmento do tamanho adequado para a operação.
    /// </summary>
    public class NoSegmentFitException : Exception
    {
        public NoSegmentFitException() : base()
        {

        }

        public NoSegmentFitException(string message) : base(message)
        {

        }
    }

    /// <summary>
    /// É levantada quando não foi encontrado um segmento do tamanho adequado para a operação.
    /// </summary>
    public class MemoryFullException : Exception
    {
        public MemoryFullException() : base()
        {

        }

        public MemoryFullException(string message) : base(message)
        {

        }
    }
}
