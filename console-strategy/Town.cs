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
        private Resource wood;
        private Resource stone;
        private Resource gold;
        private Resource[] resources = new Resource[3];
        private List<Building> buildings = new List<Building>();

        private ConsoleHandler console;

        public Town(Resource wood, Resource stone, Resource gold)
        {
            this.Wood = wood;
            this.Stone = stone;
            this.Gold = gold;

            this.console = ConsoleHandler.GetInstance();
        }

        public Resource[] Resources
        {
            get { return this.resources; }
        }
        public Resource Wood
        {
            set { this.wood = value; this.resources[0] = value; }
            get { return wood; }
        }
        public Resource Stone
        {
            set { this.stone = value; this.resources[1] = value; }
            get { return stone; }
        }
        public Resource Gold
        {
            set { this.gold = value; this.resources[2] = value; }
            get { return gold; }
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
            Dictionary<string, Command> debugList = new Dictionary<string, Command>();
            foreach (Resource resource in this.Resources)
            {
                int amount = 100;
                debugList.Add($"Add {amount} {resource.Name}", new ResourceCommand("Add Resource", this, resource, amount));
                debugList.Add($"Remove {amount} {resource.Name}", new ResourceCommand("Add Resource", this, resource, -amount));
                debugList.Add($"Add {100} Capacity for {resource.Name}", new ResourceCommand("Add Resource Capacity", this, resource, 100));
                debugList.Add($"Remove {100} Capacity from {resource.Name}", new ResourceCommand("Add Resource Capacity", this, resource, -100));

            }
            debugList.Add("Go To Town Overview", new OptionsCommand("Main Menu", this));

            this.console.UpdateConsole(this.Resources, "Change values of this town:", debugList);
        }

        public void DisplayBuildingList(string type)
        {
            Dictionary<string, Command> buildingsOptions = new Dictionary<string, Command>();

            foreach (Building building in this.buildings)
            {
                int rWood = building.RequiredResources.First(resource => resource.Name == "Wood").Amount;
                int rStone = building.RequiredResources.First(resource => resource.Name == "Stone").Amount;
                int rGold = building.RequiredResources.First(resource => resource.Name == "Gold").Amount;
                string hasEnoughResources = this.CanBuildBuilding(building) ? "" : "- Not enough resources";

                if (building.Level > 0 && type == "upgrade")
                {
                    string buildingInfo = $"{building.Name}, lvl: {building.Level}, [Wood: {rWood}, Stone: {rStone}, Gold: {rGold}] {hasEnoughResources}";
                    buildingsOptions.Add(buildingInfo, new BuildingCommand("Upgrade Building", this, building));
                }
                else if (building.Level == 0 && type == "build")
                {
                    string buildingInfo = $"{building.Name}, [Wood: {rWood}, Stone: {rStone}, Gold: {rGold}] {hasEnoughResources}";
                    buildingsOptions.Add(buildingInfo, new BuildingCommand("Build Building", this, building));
                }
                else if (type == "repair" && building.HitPoints < building.MaxHitPoints)
                {
                    buildingsOptions.Add($"{building.Name}, {building.HitPoints}/{building.MaxHitPoints}HP", new BuildingCommand("Repair Building", this, building));
                }
            }
            buildingsOptions.Add("Go To Town Overview", new OptionsCommand("Main Menu", this));

            string optHeader = type == "upgrade" ? "Choose a Building to be upgraded" : type == "build" ? "Choose a Building to be built" : "Choose a Building to be repaired";
            this.console.UpdateConsole(this.resources, "Buildings in this town:", buildingsOptions, optDescription: optHeader);
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

        public List<Building> GetBaseTownBuildings()
        {
            if (this.buildings.Count == 0)
            {
                Building barracks = new Building("Barracks", 100, 120, 60, resources, 1);
                barracks.HitPoints = 20;
                this.buildings.Add(new Building("Town Hall", 100, 120, 60, resources, 1));
                this.buildings.Add(new Building("Inn", 100, 120, 60, resources, 1));
                this.buildings.Add(new Building("Smith", 100, 120, 60, resources, 1));
                this.buildings.Add(barracks);
                this.buildings.Add(new Building("Silo", 100, 120, 60, resources, 0));
                this.buildings.Add(new Building("Market", 100, 120, 60, resources, 0));
                this.buildings.Add(new Building("Walls", 100, 120, 60, resources, 0));

                return this.buildings;
            }
            return this.buildings;
        }

    }
}
