using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_strategy
{
    internal class ConsoleHandler
    {
        private Dictionary<string, int> resources = new Dictionary<string, int>() { {"Wood", 300 }, {"Stone",500 }, { "Gold", 35} };
        private string description = "The peasants cheer as [Building Name] is erected in the town square, a symbol of prosperity and community.";
        private string[] options = { "Build", "Upgrade", "Repair", "Overview" };
        private int activeOptionIndex = 0;
        public ConsoleHandler() { }

        public string ActiveOption() { return options[activeOptionIndex]; }

        public void SelectNextOption()
        {
            if (activeOptionIndex < options.Length-1)
            {
                this.activeOptionIndex++;
            }
        }
        public void SelectPrevOption()
        {
            if (activeOptionIndex > 0)
            {
                this.activeOptionIndex--;
            }
        }
        public void ConfirmSelection()
        {
            Console.WriteLine("Selected: {0}", this.ActiveOption());
        }
        public void PrintResources()
        {
            Console.WriteLine("Your current resources:");
            for (var i=0; i< resources.Count; i++) {
                KeyValuePair<string, int> resource = resources.ElementAt(i);

                if(i == resources.Count-1)
                {
                    Console.Write("{0}: {1}\n", resource.Key, resource.Value);
                }
                else
                {
                    Console.Write("{0}: {1}, ", resource.Key, resource.Value);
                }
            } 

        }
        public void PrintDescription()
        {
            Console.WriteLine(description);

        }
        public void PrintOptions()
        {
            Console.WriteLine("\nWhat do you want to do next: ");

            for (int i = 0; i < options.Length; i++)
            {
                if (i == activeOptionIndex)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    System.Console.WriteLine(" {0}: {1} ", i, options[i]);
                    Console.ResetColor();
                }
                else
                {
                    System.Console.WriteLine(" {0}: {1} ", i, options[i]);
                }
            }
        }

        public void UpdateConsole()
        {
            Console.Clear();
            this.PrintResources();
            this.PrintDescription();
            this.PrintOptions();
        }
        public void ClearConsole()
        {
            Console.Clear();

        }
        public void ReadInput()
        {
            this.UpdateConsole();
            ConsoleKeyInfo keyInput = Console.ReadKey(true);
            while(keyInput.Key != ConsoleKey.Escape)
            {
                switch (keyInput.Key)
                {
                    case ConsoleKey.UpArrow:
                        this.SelectPrevOption(); 
                        break;
                    case ConsoleKey.DownArrow:
                        this.SelectNextOption();
                        break;
                    case ConsoleKey.Enter:
                        this.ConfirmSelection();
                        break;
                }
                this.UpdateConsole();
                keyInput = Console.ReadKey(true);
            }
        }
    }
}
