using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_strategy
{
    internal class Resource
    {
        private string name;
        private int amount;
        private int? capacity;

        public Resource(string name, int amount=0, int capacity=0) {
            this.name = name;
            this.amount = amount;
            this.capacity = capacity;
        }
        public string Name { 
            get { return name; } 
        }
        public int Amount { 
            set { this.amount = value; }
            get { return amount; } 
        }
        public int Capacity { 
            set { this.capacity = value; }
            get { return capacity ?? 0; } 
        }

        public void ChangeAmount(int value)
        {
            this.Amount += value;
        }

        public void ChangeCapacity(int value)
        {
            this.Capacity += value;
        }

    }
}
