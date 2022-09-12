namespace filesystem
{
    public class File
    {
        public string name;
        public int size;
        public int start;
        public int end;

        public File(string name, int size, int start, int end)
        {
            this.name = name;
            this.size = size;
            this.start = start;
            this.end = end;
        }

        public override string ToString()
        {
            string s = $"{this.name} \t {this.size} \t\t {Program.KBtoClusters(this.size)} \t\t\t start: {start}, end: {end}";
            return s;
        }

    }
}