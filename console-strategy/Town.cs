using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace console_strategy
{
    internal class Town
    {
        private List<Resource> resources = new List<Resource>();
        private List<Building> buildings = new List<Building>();
        private Dictionary<string, string> availableBuildings = new Dictionary<string, string>();

        private ConsoleHandler console;

        public Town(List<Resource> resources)
        {
            this.resources = resources;
            this.availableBuildings.Add("Town Hall", "medium");
            this.availableBuildings.Add("Barracks", "medium");
            this.availableBuildings.Add("Smith", "small");
            this.availableBuildings.Add("Silo", "medium");
            this.availableBuildings.Add("Market", "medium");
            this.availableBuildings.Add("Walls", "big");


            this.console = ConsoleHandler.GetInstance();
        }

        public List<Resource> Resources
        {
            get { return this.resources; }
        }


        public void IncreaseResource(Resource resource, int amount)
        {
            resource.ChangeAmount(amount);
        }
        public void IncreaseResourceCapacity(Resource resource, int amount)
        {
            resource.ChangeCapacity(amount);
        }

        public void UpgradeBuilding(Building building)
        {
            building.Upgrade();
            this.DisplayBuildingList("upgrade");
        }
        public void BuildBuilding(Building building)
        {
            building.Build();
            this.DisplayBuildingList("build");
        }
        public void RepairBuilding(Building building)
        {
            building.Repair();
            this.DisplayBuildingList("repair");
        }
        //TODO: check if there are enough resources for the given building,
        //  also if the building's level is below "maximum level"
        public bool CanBuildBuilding(Building building)
        {
            return building.CanUpgradeBuilding();
        }

        public void GoToOverviewMenu()
        {
            this.console.UpdateConsole(this.Resources, this.GetDescription("overview"), this.GetBaseOptions());
        }

        public void DisplayDebugList()
        {
            int amount = 100;
            Dictionary<string, Command> debugList = new Dictionary<string, Command>();
            foreach (Resource resource in this.Resources)
            {
                debugList.Add($"Add {amount} {resource.Name}", new ResourceCommand("Add Resource", this, resource, amount));
                debugList.Add($"Remove {amount} {resource.Name}", new ResourceCommand("Add Resource", this, resource, -amount));
                debugList.Add($"Add {amount} Capacity for {resource.Name}", new ResourceCommand("Add Resource Capacity", this, resource, amount));
                debugList.Add($"Remove {amount} Capacity from {resource.Name}", new ResourceCommand("Add Resource Capacity", this, resource, -amount));
            }
            debugList.Add("Go To Town Overview", new OptionsCommand("Main Menu", this));

            this.console.UpdateConsole(this.Resources, "Change values of this town:", debugList);
        }

        public string GetResourceInfo(Building building)
        {
            string x="";
            for (int i = 0; i < building.RequiredResources.Count; i++)
            {
                x += $"{building.RequiredResources.ElementAt(i).Name}: {building.RequiredResources.ElementAt(i).Amount}";
                if(i != building.RequiredResources.Count - 1)
                {
                    x += ", ";
                }
            }
            return x;
        }


        public string GetBuildingDesc(Building building)
        {
            string hasEnoughResources = this.CanBuildBuilding(building) ? "" : "- Not enough resources";

            string buildingInfo = $"{building.Name}, lvl: {building.Level}, ";
            string reqResourcesInfo = $"[{this.GetResourceInfo(building)}] {hasEnoughResources}";

            buildingInfo += reqResourcesInfo;

            return buildingInfo;
        }

        public void DisplayBuildingList(string type)
        {
            Dictionary<string, Command> buildingsOptions = new Dictionary<string, Command>();
            List<Building> suitableBuildings= new List<Building>();
            string commandType="";
            string desc = "";
            string optDesc = "";
            switch (type)
            {
                case "build":
                    suitableBuildings = this.buildings.Where(building => building.Level == 0).ToList();
                    desc = "Missing Buildings in this town:";
                    commandType = "Build Building";
                    optDesc = "Choose a Building to be built";
                    break;
                case "upgrade":
                    suitableBuildings = this.buildings.Where(building => building.Level > 0).ToList();
                    desc = "Buildings ready to be upgraded in this town:";
                    commandType = "Upgrade Building";
                    optDesc = "Choose a Building to be upgraded";
                    break;
                case "repair":
                    suitableBuildings = this.buildings.Where(building => building.HitPoints < building.MaxHitPoints).ToList();
                    desc = "Damaged Buildings in this town:";
                    commandType = "Repair Building";
                    optDesc = "Choose a Building to be repaired";
                    break;
                    default: 
                        throw new ArgumentException("Given 'type' is not declared.");
            }


            foreach (Building building in suitableBuildings)
            {
                string buildingInfo = GetBuildingDesc(building);
                if(type== "repair") buildingInfo = $"{building.HitPoints}/{building.MaxHitPoints} HP, " + buildingInfo;
                buildingsOptions.Add(buildingInfo, new BuildingCommand(commandType, this, building));
            }

            buildingsOptions.Add("Go To Town Overview", new OptionsCommand("Main Menu", this));

            this.console.UpdateConsole(this.resources, desc, buildingsOptions, optDescription: optDesc);
        }

        public string GetDescription(string type, Building? building = null, Resource? resource = null)
        {
            switch (type)
            {
                case "welcome":
                    return "Welcome to your town, your liege.";
                case "overview":
                    return "The town and it's people await your next move.";
                case "ending-lost":
                    return "Game over.";
                case "ending-won":
                    return "You win.";
                default:
                    return "";
            }
        }

        public Dictionary<string, Command> GetBaseOptions()
        {
            Dictionary<string, Command> baseOptions = new Dictionary<string, Command>();
            baseOptions.Add("Upgrade a Building", new OptionsCommand("Upgrade Buildings", this));
            baseOptions.Add("Build a Building", new OptionsCommand("Build Buildings", this));

            //TODO: check if there are any damaged buildings
            baseOptions.Add("Repair a Building", new OptionsCommand("Repair Buildings", this));

            //TODO: check if there are barracks in the town
            baseOptions.Add("Train Troops", new OptionsCommand("List Buildings", this));
            baseOptions.Add("Debug", new OptionsCommand("Debug", this));


            return baseOptions;
        }

        public List<Resource> RequiredResourcesForBuilding(string buildingType)
        {
            List<Resource> requiredResources = new List<Resource>();
            switch (buildingType)
            {
                case "big":
                    requiredResources.Add(new Resource(name: "Wood", amount: 130, capacity: 0));
                    requiredResources.Add(new Resource(name: "Stone", amount: 140, capacity: 0));
                    requiredResources.Add(new Resource(name: "Gold", amount: 100, capacity: 0));

                    break;
                case "medium":
                    requiredResources.Add(new Resource(name: "Wood", amount: 90, capacity: 0));
                    requiredResources.Add(new Resource(name: "Stone", amount: 80, capacity: 0));
                    requiredResources.Add(new Resource(name: "Gold", amount: 60, capacity: 0));

                    break;
                case "small":
                default:
                    requiredResources.Add(new Resource(name: "Wood", amount: 60, capacity: 0));
                    requiredResources.Add(new Resource(name: "Stone", amount: 50, capacity: 0));
                    requiredResources.Add(new Resource(name: "Gold", amount: 40, capacity: 0));
                    break;
            }

            return requiredResources;
        }
        public void GenerateBaseBuildings()
        {
            foreach (KeyValuePair<string, string> availableBuilding in this.availableBuildings)
            {
                this.buildings.Add(this.CreateBuilding(availableBuilding.Key, availableBuilding.Value));
            }
        }

        public Building CreateBuilding(string name, string type)
        {
            return new Building(name, this.RequiredResourcesForBuilding(type), this.resources, name == "Town Hall" ? 1 : 0);
        }

    }
}
