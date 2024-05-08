using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace console_strategy
{
    internal class Town
    {
        private List<Resource> resources = new List<Resource>();
        private List<Resource> resourceProduction = new List<Resource>();

        private List<Building> buildings = new List<Building>();

        private ConsoleHandler console;

        public Town(List<Resource> resources)
        {
            this.resources = resources;
            List<Resource> resourceProduction = new List<Resource>();
            this.resources.ForEach(resource => resourceProduction.Add(new Resource(resource.Name, amount: 10)));
            this.resourceProduction = resourceProduction;

            this.console = ConsoleHandler.GetInstance();
        }

        public List<Resource> Resources
        {
            set { this.resources = value; }
            get { return this.resources; }
        }
        public List<Resource> ResourceProduction
        {
            set { this.resourceProduction = value; }
            get { return this.resourceProduction; }
        }

        public List<Building> Buildings
        {
            set { this.buildings = value; }
            get { return this.buildings; }
        }

        public void ChangeResourceAmount(Resource resource, int amount)
        {
            resource.ChangeAmount(amount);
            this.console.UpdateResources(this.Resources);
        }
        public void ChangeResourceCapacity(Resource resource, int amount)
        {
            resource.ChangeCapacity(amount);
            this.console.UpdateResources(this.Resources);
        }

        public void ResourcesForBuilding(Building building)
        {
            building.RequiredResources.ForEach(resource =>
            {
                var currRes = this.Resources.First(res => res.Name == resource.Name);
                var reqAmount = building.RequiredResources.First(res => res.Name == resource.Name).Amount;
                this.ChangeResourceAmount(currRes, -reqAmount);
            });
        }

        public void UpdateResourceCapacity()
        {
            List<Storage> storageBuildings = this.Buildings.FindAll(building => building is Storage).Select(building => (Storage)building).ToList();
            storageBuildings.ForEach(building =>
            {
                Resource currRes = this.Resources.First(resource => resource == building.StorageType);
                currRes.Capacity = building.AdditionalCapacity;
            });
        }
        public async void UpgradeBuilding(Building building)
        {
            Dictionary<string, Command> options = new Dictionary<string, Command>();

            if (this.Buildings.Any(building => building.IsInProgress))
            {
                options.Add("Okay.", new OptionsCommand("Main Menu", this));
                this.console.UpdateConsole(this.Resources, this.ResourceProduction, "", options, optDescription: "There is another building in progress.");
            }
            else if (!building.CanUpgradeBuilding())
            {
                options.Add("Okay.", new OptionsCommand("Main Menu", this));
                this.console.UpdateConsole(this.Resources, this.ResourceProduction, "", options, optDescription: "You do not have enough resources.");
            }
            else
            {
                building.IsInProgress = true;
                KeyValuePair<string, int> newProgress = new KeyValuePair<string, int>(building.Name, 0);
                this.ResourcesForBuilding(building);
                this.console.UpdateProgress(newProgress);

                if(building is Storage)
                {
                    await ((Storage)building).Upgrade(this.console);
                    this.UpdateResourceCapacity();
                }
                else
                {
                    await building.Upgrade(this.console);
                }
                building.IsInProgress = false;

                options.Add("Okay.", new OptionsCommand("Main Menu", this));
                this.console.UpdateConsole(this.Resources, this.ResourceProduction, $"{building.Name} at level {building.Level} was completed.", options);
                this.console.RerenderConsole();

            }
        }

        public void RepairBuilding(Building building)
        {
            building.Repair();
            this.DisplayBuildingList("repair");
        }

        public void GoToOverviewMenu()
        {
            this.console.UpdateConsole(this.Resources, this.ResourceProduction, this.GetDescription("overview"), this.GetBaseOptions());
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

            this.console.UpdateConsole(this.Resources, this.ResourceProduction, "Change values of this town:", debugList);
        }

        public string GetResourceInfo(Building building)
        {
            string x = "";
            for (int i = 0; i < building.RequiredResources.Count; i++)
            {
                x += $"{building.RequiredResources.ElementAt(i).Name}: {building.RequiredResources.ElementAt(i).Amount}";
                if (i != building.RequiredResources.Count - 1)
                {
                    x += ", ";
                }
            }
            return x;
        }


        public string GetBuildingDesc(Building building)
        {
            string hasEnoughResources = building.CanUpgradeBuilding() ? "" : "- Not enough resources";

            string buildingInfo = $"{building.Name}, lvl: {building.Level}, ";
            string reqResourcesInfo = $"[{this.GetResourceInfo(building)}] {hasEnoughResources}";

            buildingInfo += reqResourcesInfo;

            return buildingInfo;
        }

        public void DisplayBuildingList(string type)
        {
            Dictionary<string, Command> buildingsOptions = new Dictionary<string, Command>();
            List<Building> suitableBuildings = new List<Building>();
            string commandType = "";
            string desc = "";
            string optDesc = "";
            switch (type)
            {
                case "build":
                    suitableBuildings = this.buildings.Where(building => building.Level == 0 && !building.IsInProgress).ToList();
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
                if (type == "repair") buildingInfo = $"{building.HitPoints}/{building.MaxHitPoints} HP, " + buildingInfo;
                buildingsOptions.Add(buildingInfo, new BuildingCommand(commandType, this, building));
            }

            buildingsOptions.Add("Go To Town Overview", new OptionsCommand("Main Menu", this));

            this.console.UpdateConsole(this.Resources, this.ResourceProduction, desc, buildingsOptions, optDescription: optDesc);
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
            if (this.IsAnyBuildingSpotLeft())
            {
                baseOptions.Add("Build a Building", new OptionsCommand("Build Buildings", this));
            }
            if (this.IsAnyBuildingDamaged())
            {
                baseOptions.Add("Repair a Building", new OptionsCommand("Repair Buildings", this));
            }
            if (this.IsAbleToTrainTroops())
            {
                baseOptions.Add("Train Troops", new OptionsCommand("List Buildings", this));
            }
            baseOptions.Add("Debug", new OptionsCommand("Debug", this));


            return baseOptions;
        }

        public bool IsAnyBuildingDamaged()
        {
            foreach (var building in this.Buildings)
            {
                if (building.HitPoints < building.MaxHitPoints)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsAnyBuildingSpotLeft()
        {
            foreach (var building in this.Buildings)
            {
                if (building.Level == 0)
                {
                    return true;
                }
            }
            return true;
        }
        public bool IsAbleToTrainTroops()
        {
            foreach (var building in this.Buildings)
            {
                if (building is Blacksmith && building.Level > 0)
                {
                    return true;
                }
            }
            return false;
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
            this.Buildings.Add(new Building("Town Hall", this.RequiredResourcesForBuilding("medium"), this.Resources, level: 1));
            this.Buildings.Add(new Blacksmith(this.RequiredResourcesForBuilding("small"), this.Resources, level: 1));
            this.Buildings.Add(new Storage("Wood Storage", this.RequiredResourcesForBuilding("medium"), this.Resources, this.Resources.ElementAt(0)));
            this.Buildings.Add(new Storage("Stone Storage", this.RequiredResourcesForBuilding("medium"), this.Resources, this.Resources.ElementAt(1)));
            this.Buildings.Add(new Storage("Bank", this.RequiredResourcesForBuilding("medium"), this.Resources, this.Resources.ElementAt(2), level: 2));
            this.Buildings.Add(new Barracks(this.RequiredResourcesForBuilding("medium"), this.Resources, level: 1));
            this.Buildings.Add(new Building("Market", this.RequiredResourcesForBuilding("medium"), this.Resources));
            this.Buildings.Add(new Building("Walls", this.RequiredResourcesForBuilding("big"), this.Resources));
            this.UpdateResourceCapacity();
        }
    }
}
