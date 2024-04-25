using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace console_strategy
{
    internal class Game
    {
        private ConsoleHandler console;
        private Town playersTown;

        //initialize starting town with given resources,
        public Game(int startingWood, int startingStone, int startingGold) {
            Resource wood = new Resource("Wood", startingWood, 250);
            Resource stone = new Resource("Stone", startingStone, 250);
            Resource gold = new Resource("Gold", startingGold, 250);

            string description = "The peasants cheer as [Building Name] is erected in the town square, a symbol of prosperity and community.";
            string[] options = { "Build", "Upgrade", "Repair", "Overview" };

            Resource[] resources = { wood, stone, gold };

            this.playersTown= this.CreateTown(wood, stone, gold);
            this.console =this.CreateConsole(this.playersTown.Resources, description, options);
        }

        protected ConsoleHandler CreateConsole(Resource[] resources, string description, string[] options)
        {
            return  new ConsoleHandler(resources, description, options);
        }
        protected Town CreateTown(Resource wood, Resource stone, Resource gold)
        {
            return new Town(wood, stone, gold);
        }
        public void PrintConsole()
        {
            console.ReadInput();
        }
    }
}
