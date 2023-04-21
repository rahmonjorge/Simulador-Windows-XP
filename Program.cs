using HTools;

namespace WindowsXP
{
    internal class Program
    {
        private static readonly string[] OPTIONS = { "Sistema de Arquivos", "Processos" };

        static void Main(string[] args)
        {

            Console.WriteLine("\nBem-Vindo ao Simulador do Windows XP.");
            while (true)
            {
                Console.WriteLine("Escolha uma opção ou digite '0' para sair.");
                Menu menu = new Menu(OPTIONS.ToList());
                string? option = menu.Run();

                switch (option)
                {
                    case "1":
                        WindowsXP.filesystem.FileSystemMainClass.FileSystemMain();
                        break;
                    case "2":
                        WindowsXP.Simulador.Program.SimuladorMain();
                        break;
                    case "0":
                    case "exit":
                        return;
                    default:
                        Printer.RedLn("Invalid Option.");
                        break;
                }
            }
        }
    }
}