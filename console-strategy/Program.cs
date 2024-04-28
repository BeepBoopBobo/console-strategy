// See https://aka.ms/new-console-template for more information
using console_strategy;

List<Resource> resources = new List<Resource>();
resources.Add(new Resource("Wood", 100, 250));
resources.Add(new Resource("Stone", 100, 250));
resources.Add(new Resource("Gold", 100, 250));

Game game = new Game(resources);