using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_strategy
{
    internal class Town
    {
        private Resource wood;
        private Resource stone;
        private Resource gold;
        private Resource[] resources= new Resource[3];
        public Town(Resource wood, Resource stone, Resource gold)
        {
            this.Wood = wood;
            this.Stone = stone;
            this.Gold = gold;
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

        public void increaseResource(Resource resource, int amount)
        {
            resource.ChangeAmount(amount);
        }
        public void increaseResourceCapacity(Resource resource, int amount)
        {
            resource.ChangeCapacity(amount);
        }

    }
}
