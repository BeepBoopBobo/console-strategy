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
        private Resource[] requiredResources= new Resource[3];

        private int hitPoints = 100;
        private int maxHitPoints = 100;
        private int level = 1;

        private Resource[] townResources;
        public Building(string name, int rWood, int rStone, int rGold, Resource[] townResources, int level)
        {
            this.requiredResources[0]= new Resource("Wood", rWood);
            this.requiredResources[1] = new Resource("Stone", rStone);
            this.requiredResources[2] = new Resource("Gold", rGold);

            this.Name = name;
            this.TownResources = townResources;
            this.Level = level;

        }

        public Resource[] RequiredResources { get { return this.requiredResources; } }
        public string Name { set { this.name = value; } get { return this.name; } }


        public int HitPoints { set { this.hitPoints = value; } get { return this.hitPoints; } }
        public int MaxHitPoints { set { this.maxHitPoints = value; } get { return this.maxHitPoints; } }

        public int Level { set { this.level = value; } get { return this.level; } }
        public Resource[] TownResources { set { this.townResources = value; } get { return this.townResources; } }
        public void Upgrade()
        {
            this.Level++;
        }
        public void Build()
        {
            if(this.level == 0)
            {
                this.level = 1;
            }
        }
        public void Repair()
        {
            this.HitPoints = this.maxHitPoints;
        }
        public bool hasEnoughResourcesAmount(string resourceType)
        {
            Resource available = this.TownResources.FirstOrDefault(resource => resource.Name == resourceType);
            Resource required= this.requiredResources.FirstOrDefault(resource => resource.Name == resourceType);
            if(available != null && required != null)
            {
                return available.Amount >= required.Amount;
            }
            else { return false; }
        }
        public bool CanUpgradeBuilding()
        {
            if (hasEnoughResourcesAmount("Gold") && hasEnoughResourcesAmount("Stone") && hasEnoughResourcesAmount("Wood")) { 
                return true;
            }
            else { return false; }
        }

    }
}
