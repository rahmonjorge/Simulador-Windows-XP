namespace HTools
{
    public class Menu
    {
        public string Header { get; set; } = "";
        public List<string> Items { get; set; }
        public string Capsule { get; set; } = "[]";
        public bool exit = false;

        public Menu()
        {

        }

        public Menu(List<string> items)
        {
            Items = items;
            Enumerate();
        }

        public void Enumerate()
        {
            for (int i = 1; i <= Items.Count; i++)
            {
                Items[i - 1] = Capsule[0].ToString() + i + Capsule[1].ToString() + " " + Items[i - 1];
            }
        }

        public void Print()
        {
            Printer.SystemYellow(Header);
            foreach (string item in Items) Console.WriteLine(item);
            Printer.Print("\n>> ");
        }

        public string Run()
        {
            Print();
            int input = -1;
            try
            {
                input = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {

            }
            if (input >= 0 && input <= Items.Count)
            {
                return input.ToString();
            }
            return "";
        }
    }
}