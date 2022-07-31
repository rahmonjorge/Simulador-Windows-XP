namespace Simulador
{
    public static class Utils
    {
        public static List<string> ProcessListToListStatus(List<Process> list)
        {
            List<string> s = new List<string>();
            foreach (Process p in list)
            {
                s.Add(p.Status() + "\n");
            }
            return s;
        }

        public static string ProcessListToString(List<Process> list)
        {
            string s = "";
            foreach (Process p in list)
            {
                s += p.ToString() + "\n";
            }
            return s;
        }
    }
}