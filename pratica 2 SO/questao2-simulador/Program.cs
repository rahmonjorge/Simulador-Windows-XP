/// <summary>
/// Classe implementada por Luiz Miguel e Rahmon Jorge
/// </summary>
namespace Simulador
{
    public class Program
    {
        static void Main()
        {
            bool exit = false;

            while (!exit)
            {
                Printer.PrintRainbowLn("\nBem-Vindo ao Simulador de Escalonamento de Processos!");
                Console.WriteLine("[1] Iniciar Simulador");
                Console.WriteLine("[2] Créditos");
                Console.WriteLine("[0] Encerrar");
                switch (GetChoice())
                {
                    case "1":
                        Simulador();
                        break;
                    case "2":
                        Creditos();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        break;
                }

            }
        }

        private static void Simulador()
        {
            List<Process>? processList = new List<Process>();
            Console.WriteLine("\nDeseja criar uma lista de processos ou selecionar uma lista pronta?");
            Console.WriteLine("[1] Ver listas prontas");
            Console.WriteLine("[2] Criar lista");
            Console.WriteLine("[0] Sair");
            switch (GetChoice())
            {
                case "1":
                    processList = SelecionarListaPronta();
                    if (processList == null) Simulador();
                    break;
                case "2":
                    processList = CriarLista();
                    break;
                case "0":
                    Main();
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

            Console.Write("\nEscolha o tempo do quantum (ms): ");
            int quantum = Int32.Parse(Console.ReadLine());
            Console.WriteLine("\nQual Algoritmo deseja utilizar?");
            Console.WriteLine("[1] Multiple Queues");
            Console.WriteLine("[2] Round Robin");
            switch (GetChoice())
            {
                case "1":
                    MultipleQueues mq = new MultipleQueues(quantum);
                    mq.AddProcessList(processList);
                    mq.Run();
                    break;
                case "2":
                    RoundRobin rr = new RoundRobin(processList, quantum);
                    rr.Run();
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }

        private static List<Process>? SelecionarListaPronta()
        {
            List<Process> list1 = new List<Process>()
            {
                new Process(1,"a",0,true,false,20),
                new Process(2,"b",1,false,true,15),
                new Process(3,"c",1,true,false,35),
                new Process(4,"d",2,true,false,10),
            };

            List<Process> list2 = new List<Process>()
            {
                new Process(1,"a",0,true,false,5),
                new Process(2,"b",1,false,true,40),
                new Process(3,"c",1,true,false,12),
                new Process(4,"d",3,true,false,7),
                new Process(5,"e",2,true,false,32),
                new Process(6,"f",3,true,false,55),
            };

            Console.WriteLine("\nListas disponíveis:\n");
            Console.WriteLine(" - LISTA 1 - ");
            Console.WriteLine(Utils.ProcessListToString(list1));
            Console.WriteLine(" - LISTA 2 - ");
            Console.WriteLine(Utils.ProcessListToString(list2));
            Console.WriteLine("[1] Selecionar lista 1");
            Console.WriteLine("[2] Selecionar lista 2");
            Console.WriteLine("[0] Voltar");
            switch (GetChoice())
            {
                case "1":
                    return list1;
                case "2":
                    return list2;
                case "0":
                    return null;
                default:
                    Console.WriteLine("Opção inválida.");
                    return null;
            }
        }



        private static List<Process> CriarLista()
        {
            List<Process> processList = new List<Process>();
            Console.WriteLine("Crie sua lista de Processos a seguir:");
            Console.WriteLine("Quantos Processos deseja criar?");
            int qtdProcess = Int32.Parse(Console.ReadLine());

            for (int id = 1; id <= qtdProcess; id++)
            {
                Console.WriteLine("Qual o Nome do Processo " + id + "?");
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
                processList.Add(process);
                Console.WriteLine(process);
                Console.WriteLine();
            }
            return processList;
        }

        private static void Creditos()
        {
            Console.WriteLine("\n- Créditos -");
            Console.WriteLine("Algoritmo Multiple Queues: Luiz Miguel");
            Console.WriteLine("Algoritmo Round Robin: Rahmon Jorge");
        }

        private static string? GetChoice()
        {
            Console.Write("> ");
            return Console.ReadLine();
        }
    }
}