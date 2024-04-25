// See https://aka.ms/new-console-template for more information
using console_strategy;


Resource wood = new Resource("Wood", 100, 250);
Resource stone = new Resource("Stone", 150, 250);
Resource gold = new Resource("Gold", 150, 250);

string description = "The peasants cheer as [Building Name] is erected in the town square, a symbol of prosperity and community.";
string[] options = { "Build", "Upgrade", "Repair", "Overview" };

Resource[] resources = { wood, stone, gold };
ConsoleHandler console = new ConsoleHandler(resources, description, options);

console.ReadInput();