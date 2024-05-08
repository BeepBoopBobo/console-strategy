using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace console_strategy
{
    internal abstract class Equipment
    {
        private string name;
        private int level;
        private Building buildingRequirement;
        private Resource resourceRequirements;


        public string Name { get { return this.name; } set { this.name = value; } }
        public int Level { get { return this.level; } set { this.level = value; } }
        public Building BuildingRequirement { get { return this.buildingRequirement; } set { this.buildingRequirement = value; } }
        public Resource ResourceRequirements { get { return this.resourceRequirements; } set { this.resourceRequirements = value; } }

        public Equipment(string name, int level, Building buildingRequirement, Resource resourceRequirements)
        {
            this.Name = name;
            this.Level = level;
            this.buildingRequirement = buildingRequirement;
            this.resourceRequirements = resourceRequirements;
        }

    }
    internal class Weapon : Equipment
    {
        private int attack;
        private int attackRange;

        public int Attack { get { return this.attack; } set { this.attack = value; } }
        public int AttackRange { get { return this.attackRange; } set { this.attackRange = value; } }

        public Weapon(string name, int level, Building buildingRequirement, Resource resourceRequirements,int attack, int attackRange): base(name, level,buildingRequirement, resourceRequirements)
        {
            this.Attack = attack;
            this.AttackRange = attackRange;

        }
    }
    internal class Armor : Equipment
    {
        private int defense;
        public int Defense { get { return this.defense; } set { this.defense = value; } }

        public Armor(string name, int level, Building buildingRequirement, Resource resourceRequirements, int armor) : base(name, level, buildingRequirement, resourceRequirements)
        {
            this.Defense = armor;
        }

    }

}
