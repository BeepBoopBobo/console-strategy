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

        private List<Resource> townResources;
        public Building(string name, List<Resource> requiredResources, List<Resource> townResources, int level)
        {
            this.requiredResources = requiredResources;


            this.Name = name;
            this.TownResources = townResources;
            this.Level = level;

        }

        public List<Resource> RequiredResources { get { return this.requiredResources; } }
        public string Name { set { this.name = value; } get { return this.name; } }


        public int HitPoints { set { this.hitPoints = value; } get { return this.hitPoints; } }
        public int MaxHitPoints { set { this.maxHitPoints = value; } get { return this.maxHitPoints; } }

        public int Level { set { this.level = value; } get { return this.level; } }
        public List<Resource> TownResources { set { this.townResources = value; } get { return this.townResources; } }
        public void Upgrade()
        {
            this.Level++;
        }
        public void Build()
        {
            if (this.level == 0)
            {
                this.level = 1;
            }
        }
        public void Repair()
        {
            this.HitPoints = this.maxHitPoints;
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
}
