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
        private int round = 0;
        private ConsoleHandler console;
        public Town playersTown;

        //initialize starting town with given resources,
        public Game(int startingWood, int startingStone, int startingGold) {
            Resource wood = new Resource("Wood", startingWood, 250);
            Resource stone = new Resource("Stone", startingStone, 250);
            Resource gold = new Resource("Gold", startingGold, 250);

            this.playersTown= this.CreateTown(wood, stone, gold);
            this.playersTown.GetBaseTownBuildings();
            this.console = ConsoleHandler.GetInstance();

            this.console.UpdateConsole(this.playersTown.Resources , this.playersTown.GetDescription("welcome"), this.playersTown.GetBaseOptions());
            this.console.ReadInput();
        }


        protected Town CreateTown(Resource wood, Resource stone, Resource gold)
        {
            return new Town(wood, stone, gold);
        }
        public void increaseRound()
        {
            this.round++;
        }
    }
}
