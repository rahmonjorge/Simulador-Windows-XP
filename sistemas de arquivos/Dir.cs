namespace filesystem
{
    public class Dir
    {
        public string name;
        public List<File> contents;
        public int location;

        public Dir(string name, int location)
        {
            this.name = name;
            this.location = location;
            contents = new List<File>();
        }

        public void ListFiles()
        {
            foreach (File file in contents)
            {
                Console.WriteLine(file.name);
            }
        }

        public override string ToString()
        {
            string s = $"[{this.name}] \t {1} \t\t {1} \t\t\t start: {location}, end: {location}";
            return s;
        }
    }

}
