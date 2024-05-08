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
        private bool isGameOver = false;

        private ConsoleHandler console;
        private List<Resource> usedResources;
        public Town playersTown;

        //initialize starting town with given resources,
        public Game(List<Resource> resources)
        {
            this.usedResources = resources;
            this.playersTown = this.CreateTown();
            this.playersTown.GenerateBaseBuildings();
            this.console = ConsoleHandler.GetInstance();
            this.playersTown.GoToOverviewMenu();
            this.StartGeneratingResources();
            this.console.ReadInput();
        }


        protected Town CreateTown()
        {
            return new Town(this.usedResources);
        }

        public async void StartGeneratingResources()
        {
            await this.AwardResources();
        }
        async Task AwardResources()
        {
            while (!this.isGameOver)
            {
                await Task.Delay(1000);
                this.usedResources.ForEach(resource =>
                {
                    int resAmount = this.playersTown.ResourceProduction.First(res => res.Name == resource.Name).Amount;
                    this.playersTown.ChangeResourceAmount(resource, resAmount);
                });
            }
        } 
        public void IncreaseRound()
        {
            this.round++;
        }
    }
}
