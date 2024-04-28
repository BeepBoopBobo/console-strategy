
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
                case "Debug":
                    this.recieverTown.DisplayDebugList();
                    break;
                case "Main Menu":
                    this.recieverTown.GoToOverviewMenu();
                    break;
                default: break;
            }
        }
    }

    internal class ResourceCommand : Command
    {
        private string command;
        private int amount;
        private Resource resource;
        private Town recieverTown;

        public ResourceCommand(string command, Town recieverTown, Resource resource, int amount)
        {
            this.command = command;
            this.recieverTown = recieverTown;
            this.amount = amount;
            this.resource = resource;
        }

        public override void Execute()
        {
            switch (this.command)
            {
                case "Add Resource":
                    this.recieverTown.IncreaseResource(this.resource, this.amount);
                    break;
                case "Add Resource Capacity":
                    this.recieverTown.IncreaseResourceCapacity(this.resource, this.amount);
                    break;
                default: break;
            }
        }
    }

    internal class BuildingCommand : Command
    {
        private string command;
        private Building building;
        private Town recieverTown;

        public BuildingCommand(string command, Town recieverTown, Building building)
        {
            this.command = command;
            this.recieverTown = recieverTown;
            this.building = building;
        }

        public override void Execute()
        {
            switch (this.command)
            {
                case "Upgrade Building":
                    this.recieverTown.UpgradeBuilding(this.building);
                    break;
                case "Build Building":
                    this.recieverTown.BuildBuilding(this.building);
                    break;
                case "Repair Building":
                    this.recieverTown.RepairBuilding(this.building);
                    break;
                default: break;
            }
        }
    }
}
