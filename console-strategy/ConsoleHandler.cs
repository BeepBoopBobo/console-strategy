using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_strategy
{
    internal class ConsoleHandler
    {
        private static ConsoleHandler instance;
        private int activeOptionIndex = 0;

        private string description;
        private string optionDescription;
        private Dictionary<string, Command> options;
        private Resource[] resources;

        public ConsoleHandler()
        {
        }

        public static ConsoleHandler GetInstance()
        {
            if (instance == null)
            {
                instance = new ConsoleHandler();
            }
            return instance;
        }

        public int ActiveOptionIndex
        {
            set { activeOptionIndex = value; }
            get { return activeOptionIndex; }
        }
        public string Description
        {
            set { this.description = value; }
            get { return this.description; }
        }
        public string OptionDescription
        {
            set { this.optionDescription = value; }
            get { return this.optionDescription; }
        }
        public Dictionary<string, Command> Options
        {
            set { this.options = value; }
            get { return this.options; }
        }
        public Resource[] Resources
        {
            set { this.resources = value; }
            get { return this.resources; }
        }


        public string ActiveOption() { return this.Options.ElementAt(activeOptionIndex).Key; }

        public void UpdateResources(Resource[] resources)
        {
            this.Resources = resources;
            this.RerenderConsole();
        }
        public void UpdateDescription(string description)
        {
            this.Description = description;
            this.RerenderConsole();
        }
        public void UpdateOptions(Dictionary<string, Command> options)
        {
            this.Options = options;
            this.RerenderConsole();
        }
        public void UpdateConsole(Resource[] resources, string description, Dictionary<string, Command> options, int activeOptionIndex = 0, string optDescription = "")
        {
            this.Resources = resources;
            this.Description = description;
            this.optionDescription = optDescription;
            this.Options = options;
            this.ActiveOptionIndex = activeOptionIndex;
        }

        public void SelectNextOption()
        {
            if (this.ActiveOptionIndex < this.Options.Count - 1)
            {
                this.ActiveOptionIndex++;
            }
        }
        public void SelectPrevOption()
        {
            if (this.ActiveOptionIndex > 0)
            {
                this.ActiveOptionIndex--;
            }
        }
        public void ConfirmSelection()
        {
            this.Options.ElementAt(this.ActiveOptionIndex).Value.Execute();
        }
        public void PrintResources()
        {
            Console.WriteLine("Your current resources:");
            for (var i = 0; i < this.Resources.Length; i++)
            {

                if (i == this.Resources.Length - 1)
                {
                    Console.Write("{0}: {1}/{2}\n", this.Resources[i].Name, this.Resources[i].Amount, this.Resources[i].Capacity);
                }
                else
                {
                    Console.Write("{0}: {1}/{2}, ", this.Resources[i].Name, this.Resources[i].Amount, this.Resources[i].Capacity);
                }
            }
        }
        public void PrintDescription()
        {
            Console.WriteLine(description);
        }
        public void PrintOptions()
        {
            string optHeader = this.OptionDescription != "" ? this.OptionDescription : "What do you want to do next";

            Console.WriteLine($"\n{optHeader}: ");

            for (int i = 0; i < this.Options.Count; i++)
            {
                if (i == this.ActiveOptionIndex)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(" {0}: {1}", i, this.Options.ElementAt(i).Key);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(" {0}: {1}", i, this.Options.ElementAt(i).Key);
                }
            }
        }


        public void RerenderConsole()
        {
            this.ClearConsole();
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
            this.RerenderConsole();
            ConsoleKeyInfo keyInput = Console.ReadKey(true);
            while (keyInput.Key != ConsoleKey.Escape)
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
                this.RerenderConsole();
                keyInput = Console.ReadKey(true);
            }
        }

    }
}