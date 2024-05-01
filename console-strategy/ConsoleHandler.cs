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
        private KeyValuePair<string, int> progress;
        private Dictionary<string, Command> options;
        private List<Resource> resources;

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
        public KeyValuePair<string, int> Progress
        {
            set { this.progress = value; }
            get { return this.progress; }
        }
        public Dictionary<string, Command> Options
        {
            set { this.options = value; }
            get { return this.options; }
        }
        public List<Resource> Resources
        {
            set { this.resources = value; }
            get { return this.resources; }
        }


        public string ActiveOption() { return this.Options.ElementAt(activeOptionIndex).Key; }

        public void UpdateResources(List<Resource> resources)
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
        public void UpdateProgress(KeyValuePair<string, int> progress)
        {
            this.Progress = progress;
            this.RerenderConsole();
        }
        public void UpdateProgressTick(int newTick)
        {
            KeyValuePair<string, int> progress = new KeyValuePair<string, int>(this.Progress.Key, newTick);
            this.Progress = progress;
            this.RerenderConsole();
        }

        public void UpdateConsole(List<Resource> resources, string description, Dictionary<string, Command> options, int activeOptionIndex = 0, string optDescription = "")
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
            for (var i = 0; i < this.Resources.Count; i++)
            {

                if (i == this.Resources.Count - 1)
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
            Console.WriteLine(this.Description);
        }
        public void PrintProgress()
        {
            string progressBar = "";
            for (var i = 1; i <= 10; i++)
            {
                progressBar += i <= this.progress.Value ? $"■" : " ";
            }
            if (this.progress.Key != "" && this.progress.Value < 10)
            {
                Console.Write($"\n{this.progress.Key}: \t[{progressBar}]");
            }
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
            if (this.Progress.Key != "") this.PrintProgress();
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