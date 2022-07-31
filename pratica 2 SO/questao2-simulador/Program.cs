namespace Simulador
{
    public class Program
    {
        static void Main()
        {
            List<Process> listProcess = new List<Process>();
            Console.WriteLine("Crie sua lista de Processos a seguir:");
            Console.WriteLine("Quantos Processos deseja criar?");
            int qtdProcess = Int32.Parse(Console.ReadLine());

            for (int id = 1; id <= qtdProcess; id++)
            {
                Console.WriteLine("Qual o Nome do Processo?");
                string name = Console.ReadLine();
                Console.WriteLine("Qual a Prioridade do Processo?");
                int priority = Int32.Parse(Console.ReadLine());
                Console.WriteLine("Ele é CPU ou IO Bound?");
                string boundStr = Console.ReadLine();
                Console.WriteLine("Qual o tempo de CPU do Processo?");
                int cpuTime = Int32.Parse(Console.ReadLine());
                bool cpuBound = boundStr.ToUpper() == "CPU";
                bool ioBound = boundStr.ToUpper() == "IO";

                Process process = new Process(id, name, priority, ioBound, cpuBound, cpuTime);
                listProcess.Add(process);
                Console.WriteLine(process);
                Console.WriteLine();
            }
            Console.WriteLine("Qual o tempo do quantum da preempção (ms)?");
            int timePreemp = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Qual Algoritmo deseja utilizar?\n1 - MultipleQueues\n2 - ?????????\n3 - ???????????");
            int algoChoice = Int32.Parse(Console.ReadLine());
            bool exit = false;
            while (!exit)
            {
                switch (algoChoice)
                {
                    case 1:
                        MultipleQueues algo = new MultipleQueues(timePreemp);
                        algo.AddProcessList(listProcess);
                        algo.Run();
                        exit = true;
                        break;
                    case 2:
                        exit = true;
                        break;
                    case 3:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Escolha de novo...");
                        break;
                }
            }
        }
    }
}