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
        private List<Resource> usedResources;
        public Town playersTown;

        //initialize starting town with given resources,
        public Game(List<Resource> resources)
        {
            this.usedResources = resources;


            this.playersTown = this.CreateTown();
            this.playersTown.GetBaseTownBuildings();
            this.console = ConsoleHandler.GetInstance();

            this.console.UpdateConsole(this.playersTown.Resources, this.playersTown.GetDescription("welcome"), this.playersTown.GetBaseOptions());
            this.console.ReadInput();
        }


        protected Town CreateTown()
        {
            return new Town(this.usedResources);
        }
        public void IncreaseRound()
        {
            this.round++;
        }
    }
}
