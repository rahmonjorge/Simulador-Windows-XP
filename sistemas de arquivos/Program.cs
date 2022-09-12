using System.IO;
using HTools;

namespace filesystem
{
    public class Program
    {
        public static bool exit = false;
        public static string? input = "";
        public static string[]? args;
        public static string cmdPointer = ">> ";
        public static string msgPointer = "\n[OK]\n";

        public static List<File> files = new List<File>();
        public static List<Dir> dirs = new List<Dir>();
        public static ConsoleColor[] clusters;

        public static FileSystem fileSystem;

        // public static int FAT12.clusterSize = 8;
        // public static int FAT12.totalClusterCount;
        // public static int FAT12.usableClusterCount;

        // public static int FAT12.freeClusters = 0;
        // public static int FAT12.usedClusters = 0;
        // public static int FAT12.newfilePointer = 2;

        // public static int FAT12.unoccupiedSpace = 0;
        // public static int FAT12.usedSpace = 0;


        public static ConsoleColor reservedCluster = ConsoleColor.Magenta;
        public static ConsoleColor freeCluster = ConsoleColor.Green;
        public static ConsoleColor occupiedCluster = ConsoleColor.Blue;
        public static ConsoleColor filePointer = ConsoleColor.Yellow;

        public static string currentPath = "C:/";
        public static Dir? currentDir;
        public static bool inRoot = true;

        public static void Main()
        {

            while (!exit)
            {
                fileSystem = new FileSystem(8, 4096, 4078, 2);
                RunSimulator();
            }
            Printer.Print("\nMade by ");
            Printer.PrintRainbow("Rahmon Jorge");
            Printer.Print(" in September 2022.\n\n");
        }

        public static void RunSimulator()
        {
            clusters = GenerateClusterArray(fileSystem.totalClusterCount);

            // Printing
            while (!exit)
            {
                Console.Clear();
                Printer.SystemYellow("- FAT12 FILE SYSTEM MANAGER - ");
                Printer.PrintLn($"Cluster Size: {fileSystem.clusterSize}KB");
                Printer.PrintLn($"Cluster Count: {fileSystem.totalClusterCount}");
                Printer.PrintLn($"Usable Cluster Count: {fileSystem.usableClusterCount}");
                Printer.PrintLn("Volume Size: 32768KB (32MB)\n");

                Printer.PrintColorLn("Reserved by system: 144KB, 18 clusters", reservedCluster);
                Printer.PrintColorLn($"Free space: {fileSystem.unoccupiedSpace}KB, {fileSystem.freeClusters} clusters", freeCluster);
                Printer.PrintColorLn($"Occupied space: {fileSystem.usedSpace}KB, {fileSystem.usedClusters} clusters\n", occupiedCluster);

                PrintClusters();

                PrintFileTree(currentPath);

                // Reading
                input = GetInput();
                args = input.Split(" ");
                switch (args[0])
                {
                    case "delete":
                        Delete();
                        break;
                    case "deleteall":
                    case "reset":
                        Reset();
                        break;
                    case "exit":
                        exit = true;
                        break;
                    case "fat":
                        VolumeInfo();
                        break;
                    case "fileinfo":
                        FileInfo();
                        break;
                    case "goto":
                        Goto();
                        break;
                    case "help":
                        Help();
                        break;
                    case "newfile":
                        CreateFile();
                        break;
                    case "newdir":
                        CreateDir();
                        break;
                    case "return":
                    case "back":
                        Return();
                        break;
                    default:
                        Printer.Red("Unknown Command: '" + args[0] + "'");
                        GetInput(msgPointer);
                        break;
                }
            }
        }

        public static void CreateDir()
        {
            if (args.Length < 2)
            {
                Printer.RedLn("Too few arguments.");
                Printer.RedLn("Usage: newdir <name>");
                GetInput(msgPointer);
                return;
            }

            string dirname = args[1];
            if (inRoot && dirs.Find(dir => dir.name == dirname) != null)
            {
                Printer.RedLn("Directory name already exists.");
                GetInput(msgPointer);
                return;
            }

            Dir newdir = new Dir(dirname, fileSystem.newfilePointer);
            dirs.Add(newdir);

            fileSystem.newfilePointer += 1;

            // Update Space
            fileSystem.usedSpace += 1;
            fileSystem.unoccupiedSpace -= 1;

            // Update Clusters
            fileSystem.usedClusters += 1;
            fileSystem.freeClusters -= 1;

            clusters[newdir.location] = occupiedCluster;

            Console.Clear();
        }

        public static void CreateFile()
        {
            if (args.Length < 3)
            {
                Printer.RedLn("Too few arguments.");
                Printer.RedLn("Usage: newfile <filename> <size> ");
                GetInput(msgPointer);
                return;
            }

            string filename;
            int size;
            int sizeInClusters;
            int start;
            int end;

            try
            {
                filename = args[1];

                if (inRoot && files.Find(file => file.name == filename) != null
                || currentDir != null && currentDir.contents.Find(file => file.name == filename) != null)
                {
                    Printer.RedLn("Filename already exists.");
                    GetInput(msgPointer);
                    return;
                }

                size = int.Parse(args[2]);

                if (size > fileSystem.unoccupiedSpace)
                {
                    Printer.RedLn("File is too large.");
                    GetInput(msgPointer);
                    return;
                }
                sizeInClusters = KBtoClusters(size);
                start = fileSystem.newfilePointer;
                end = fileSystem.newfilePointer + sizeInClusters - 1;
            }
            catch (Exception e)
            {
                Printer.RedLn("The command: [" + string.Join(",", args) + "] could not be executed.");
                Printer.RedLn(e.ToString());
                GetInput(msgPointer);
                return;
            }


            File newfile = new File(filename, size, start, end);
            if (inRoot) files.Add(newfile);
            else if (currentDir != null) currentDir.contents.Add(newfile);

            fileSystem.newfilePointer = end + 1;

            // Update Space
            fileSystem.usedSpace += newfile.size;
            fileSystem.unoccupiedSpace -= newfile.size;

            // Update Clusters
            fileSystem.usedClusters += sizeInClusters;
            fileSystem.freeClusters -= sizeInClusters;
            for (int i = start; i <= end; i++) clusters[i] = occupiedCluster;
            Console.Clear();
        }

        public static void Delete()
        {
            string name = args[1];

            File? fileToRemove;

            // Deleting file in the root
            if (inRoot)
            {
                fileToRemove = files.Find(file => file.name == name);

                if (fileToRemove != null)
                {
                    files.Remove(fileToRemove);
                    RemoveFileFromGUI(fileToRemove);
                }
                // Deleting dir in the root
                else
                {
                    Dir? dirToRemove = dirs.Find(dir => dir.name == name);

                    if (dirToRemove != null)
                    {
                        foreach (File f in dirToRemove.contents) RemoveFileFromGUI(f);
                        dirs.Remove(dirToRemove);

                        fileSystem.usedSpace--;
                        fileSystem.unoccupiedSpace++;
                        fileSystem.usedClusters--;
                        fileSystem.freeClusters++;
                        clusters[dirToRemove.location] = freeCluster;
                    }
                }

            }
            // Deleting file inside directory
            else if (currentDir != null)
            {
                fileToRemove = currentDir.contents.Find(file => file.name == name);

                if (fileToRemove != null)
                {
                    currentDir.contents.Remove(fileToRemove);
                    RemoveFileFromGUI(fileToRemove);
                }
            }
            else
            {
                Printer.RedLn("Current dir is null.");
                return;
            }

            Console.Clear();
        }

        public static void FileInfo()
        {
            if (args.Length < 2)
            {
                Printer.RedLn("Too few arguments.");
                Printer.RedLn("Usage: frag <filename>");
                GetInput(msgPointer);
                return;
            }

            File? file = files.Find(file => file.name == args[1]);
            if (file == null)
            {
                Printer.RedLn($"File '{args[1]}' not found.");
                GetInput(msgPointer);
                return;
            }

            int clusters = file.end - file.start + 1;
            Printer.PrintLn($"\nName: {file.name}");
            Printer.PrintLn($"Size: {file.size}KB");
            Printer.PrintLn($"Clusters Occupied: @{file.start} to @{file.end} ({clusters} clusters)");
            for (int i = 1; i < clusters; i++) Printer.Blue("|");

            int fragmentation = file.size % fileSystem.clusterSize;

            if (fragmentation == 0)
            {
                Printer.Blue("|");
                Printer.PrintLn("\n\nInternal Fragmentation: None");
            }
            else
            {
                Printer.DarkGreen("|");
                Printer.PrintLn($"\nInternal Fragmentation of cluster @{file.end} ({fragmentation} of {fileSystem.clusterSize}KB used):");
                Printer.Print("[");
                for (int i = 0; i < fragmentation; i++) Printer.Red("|");
                for (int i = 0; i < fileSystem.clusterSize - fragmentation; i++) Printer.Yellow("|");
                Printer.Print("]");
            }
            GetInput(msgPointer);

        }

        public static ConsoleColor[] GenerateClusterArray(int size)
        {
            ConsoleColor[] c = new ConsoleColor[size];

            c[0] = reservedCluster;
            c[1] = reservedCluster;

            int reservedStart = size - 17;
            int reservedEnd = size - 1; // Inclusive

            for (int i = 2; i < reservedStart; i++)
            {
                c[i] = freeCluster;
            }

            // Reserved clusters
            for (int i = reservedStart; i <= reservedEnd; i++)
            {
                c[i] = reservedCluster;
            }

            fileSystem.newfilePointer = 2;

            return c;
        }

        public static void Goto()
        {
            string name = args[1];

            Dir? current = dirs.Find(dir => dir.name == name);

            if (current != null)
            {
                currentPath += current.name;
                currentDir = current;
                inRoot = false;
            }
            else return;
        }

        public static void PrintClusters()
        {
            clusters[fileSystem.newfilePointer] = filePointer;

            Printer.PrintLn("Volume Allocation (1 bar = 1 cluster):");

            for (int i = 0; i < clusters.Length; i++)
            {
                Printer.PrintColor("|", clusters[i]);
            }
        }

        public static void PrintFileTree(string path)
        {
            Printer.Print("\n\nPath: ");
            Printer.Print(path);
            Printer.PrintLn();

            if (inRoot)
            {
                foreach (Dir dir in dirs) Console.WriteLine($"[{dir.name}]");
                foreach (File file in files) Console.WriteLine(file.name);
            }
            else if (currentDir != null) currentDir.ListFiles();
            else Printer.RedLn("Current dir is null.");


            Printer.PrintLn();
        }

        public static void Reset()
        {
            // Clear lists
            files.Clear();
            dirs.Clear();

            // Update Space
            fileSystem.usedSpace = 0;
            fileSystem.unoccupiedSpace = fileSystem.usableClusterCount * 8;

            // Update Clusters
            fileSystem.usedClusters = 0;
            fileSystem.freeClusters = fileSystem.usableClusterCount;

            // New Cluster
            clusters = GenerateClusterArray(fileSystem.totalClusterCount);

            Console.Clear();
        }

        private static void RemoveFileFromGUI(File fileToRemove)
        {
            // Update Space
            fileSystem.usedSpace -= fileToRemove.size;
            fileSystem.unoccupiedSpace += fileToRemove.size;

            // Update Clusters
            int sizeInClusters = KBtoClusters(fileToRemove.size);
            fileSystem.usedClusters -= sizeInClusters;
            fileSystem.freeClusters += sizeInClusters;
            for (int i = fileToRemove.start; i <= fileToRemove.end; i++) clusters[i] = freeCluster;
        }

        public static void Return()
        {
            if (!inRoot)
            {
                currentPath = currentPath.Remove(currentPath.IndexOf("/") + 1);
                currentDir = null;
                inRoot = true;
            }
        }

        /// <summary>
        /// Converts a value from KB to clusters count.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int KBtoClusters(int value)
        {
            return (int)Math.Ceiling((double)value / (double)fileSystem.clusterSize);
        }

        public static void Help()
        {
            Printer.PrintLn(System.IO.File.ReadAllText("help.txt"));
            GetInput(msgPointer);
        }

        public static string? GetInput()
        {
            return GetInput(cmdPointer);
        }
        public static string? GetInput(string s)
        {
            Printer.Print(s);
            return Console.ReadLine();
        }

        public static void VolumeInfo()
        {
            Printer.PrintLn("NAME \t SIZE (KB) \t SIZE (Clusters) \t Location \t ");

            foreach (Dir dir in dirs)
            {
                Console.WriteLine(dir);
                foreach (File file in dir.contents) Console.WriteLine(file);
            }
            foreach (File file in files)
            {
                Console.WriteLine(file);
            }
            Printer.PrintLn();
            GetInput(msgPointer);
        }
    }
}