using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_strategy
{
    internal class ConsoleHandler
    {

        private int activeOptionIndex = 0;
        private string description;
        private string[] options;
        private Resource[] resources;

        public ConsoleHandler(Resource[] resources, string description, string[] options, int activeOptionIndex=0)
        {
            this.resources = resources;
            this.description = description;
            this.options = options;
            this.activeOptionIndex = activeOptionIndex;
        }

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
            for (var i=0; i< resources.Length; i++) {

                if(i == resources.Length-1)
                {
                    Console.Write("{0}: {1}/{2}\n", resources[i].Name, resources[i].Amount, resources[i].Capacity);
                }
                else
                {
                    Console.Write("{0}: {1}/{2}, ", resources[i].Name, resources[i].Amount, resources[i].Capacity);
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
