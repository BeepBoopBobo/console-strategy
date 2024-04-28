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

        public Resource(string name, int amount = 0, int capacity = 0)
        {
            this.name = name;
            this.amount = amount;
            this.capacity = capacity;
        }
        public string Name
        {
            get { return name; }
        }
        public int Amount
        {
            set { this.amount = value; }
            get { return amount; }
        }
        public int Capacity
        {
            set { this.capacity = value; }
            get { return capacity ?? 0; }
        }

        public void ChangeAmount(int value)
        {
            if (this.Amount + value > this.capacity)
            {
                this.Amount = this.Capacity;
            }
            else if (this.Amount + value <= 0)
            {
                this.Amount = 0;
            }
            else
            {
                this.Amount += value;
            }
        }

        public void ChangeCapacity(int value)
        {
            if (this.Capacity + value <= 0)
            {
                this.Capacity = 0;
                this.Amount = 0;
            }
            else if (this.Capacity + value < this.Amount)
            {
                this.Capacity += value;
                this.Amount = this.Capacity;
            }
            else
            {
                this.Capacity += value;

            }
        }

    }
}
