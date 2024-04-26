
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_strategy
{
    internal abstract class Command
    {
        public abstract void Execute();
    }

    internal class OptionsCommand : Command
    {
        private string command;
        private Town recieverTown;

        public OptionsCommand(string command, Town recieverTown)
        {
            this.command = command;
            this.recieverTown = recieverTown;
        }

        public override void Execute()
        {
            switch (this.command)
            {
                case "Upgrade Buildings":
                    this.recieverTown.DisplayBuildingList("upgrade");
                    break;
                case "Build Buildings":
                    this.recieverTown.DisplayBuildingList("build");
                    break;
                case "Repair Buildings":
                    this.recieverTown.DisplayBuildingList("repair");
                    break;
                case "Update":
                    break;
                case "Main Menu":
                    this.recieverTown.GoToOverviewMenu();
                    break;
                    default: break;
            }
        }
    }
}
