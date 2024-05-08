using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_strategy
{
    internal class Building
    {
        private string name;
        private List<Resource> requiredResources;

        private int hitPoints = 100;
        private int maxHitPoints = 100;
        private int level = 1;
        private bool isInProgress= false;

        private List<Resource> townResources;
        public Building(string name, List<Resource> requiredResources, List<Resource> townResources, int level=0)
        {
            this.RequiredResources = requiredResources;
            this.Name = name;
            this.TownResources = townResources;
            this.Level = level;
        }


        public List<Resource> RequiredResources { set { this.requiredResources = value; } get { return this.requiredResources; } }
        public string Name { set { this.name = value; } get { return this.name; } } 

        public bool IsInProgress { set { this.isInProgress = value; }  get { return this.isInProgress; } }
        public int HitPoints { set { this.hitPoints = value; } get { return this.hitPoints; } }
        public int MaxHitPoints { set { this.maxHitPoints = value; } get { return this.maxHitPoints; } }

        public int Level { set { this.level = value; } get { return this.level; } }
        public List<Resource> TownResources { set { this.townResources = value; } get { return this.townResources; } }
        public async Task Upgrade(ConsoleHandler console)
        {
            await this.StartProcess(console);
            this.Level++;
        }

        public void Repair()
        {
            this.HitPoints = this.maxHitPoints;
        }
        public int SumResources()
        {
            int sum= 0;
            foreach (var resource in this.RequiredResources)
            {
                sum += resource.Amount;
            }
            return sum;
        }

        public async Task StartProcess(ConsoleHandler console)
        {
            for (int i= 0; i<= 10; i++)
            {
                await Task.Delay(this.SumResources());
                console.UpdateProgressTick(i);
            }
        }
        public bool HasEnoughResourcesAmount(string resourceType)
        {
            Resource available = this.TownResources.First(resource => resource.Name == resourceType);
            Resource required = this.requiredResources.First(resource => resource.Name == resourceType);
            if (available != null && required != null)
            {
                return available.Amount >= required.Amount;
            }
            else { return false; }
        }
        public bool CanUpgradeBuilding()
        {
            if (HasEnoughResourcesAmount("Gold") && HasEnoughResourcesAmount("Stone") && HasEnoughResourcesAmount("Wood"))
            {
                return true;
            }
            else { return false; }
        }

    }

    internal class Storage : Building
    {
        private int additionalCapacity;
        private Resource storageType;

        public Storage(string name, List<Resource> requiredResources, List<Resource> townResources, Resource storageType, int level = 0): 
            base(name, requiredResources, townResources, level)
        {
            this.StorageType = storageType;
            this.additionalCapacity = this.CalculateStorage();
        }

        public int AdditionalCapacity
        {
            set { this.additionalCapacity = value; }
            get { return this.additionalCapacity; }
        }
        public Resource StorageType
        {
            set { this.storageType = value; }
            get { return this.storageType; }
        }

        private int CalculateStorage()
        {
            int sum = 100;
            for (int i = 0;i<=this.Level; i++) {
                sum = sum + i * 250;
            }
            return sum;
        }

        public new async Task Upgrade(ConsoleHandler console)
        {
            await base.Upgrade(console);
            this.AdditionalCapacity = this.CalculateStorage();
        }
    }
    internal class Production: Building
    {
        private int additionalProduction;
        private Resource productionType;
        public Production(string name, List<Resource> requiredResources, List<Resource> townResources, Resource productionType, int level = 0) :
    base(name, requiredResources, townResources, level)
        {
            this.ProductionType = productionType;
            this.additionalProduction = this.CalculateProduction();
        }

        public int AdditionalProduction
        {
            set { this.additionalProduction = value; }
            get { return this.additionalProduction; }
        }
        public Resource ProductionType
        {
            set { this.productionType = value; }
            get { return this.productionType; }
        }
        private int CalculateProduction()
        {
            int sum = this.Level==0?10 :0;
            for (int i = 0; i <= this.Level; i++)
            {
                sum = sum + i * 25;
            }
            return sum;
        }

        public new async Task Upgrade(ConsoleHandler console)
        {
            await base.Upgrade(console);
            this.AdditionalProduction = this.CalculateProduction();
        }
    }
    internal class Barracks : Building
    {
        private bool canTrainTroops;

        public Barracks(List<Resource> requiredResources, List<Resource> townResources, int level = 0, string name = "Barracks") : 
            base(name, requiredResources, townResources, level)
        {
            this.canTrainTroops = this.CanTrainTroops();
        }

        public bool CanTrainTroops()
        {
            return this.Level > 0 && this.HitPoints > 0;
        }
    }

    internal class Blacksmith: Building
    {
        private bool canCreateEquipment;

        public Blacksmith(List<Resource> requiredResources, List<Resource> townResources, int level = 0, string name = "Blacksmith") :
            base(name, requiredResources, townResources, level)
        {
            this.canCreateEquipment = this.Level>0 && this.HitPoints>0;
        }

        //TODO: implement equipment class and change the string to Equipment
        public bool CanCreateSpecificItem(string type="")
        {
            switch (type)
            {
                case "plate armor":
                case "halberd":
                    return this.Level >= 3 && this.HitPoints > 0;
                case "mail armor":
                case "crossbow":
                case "spear":
                case "shield":
                    return this.Level >= 2 && this.HitPoints > 0;
                case "leather armor":
                case "sword":
                case "bow":
                    return this.Level >= 1 && this.HitPoints > 0;
                default:
                    return this.Level > 0 && this.HitPoints > 0;
            }
            
        }
    }
}
