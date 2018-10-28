using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatNew
{
    class Snack : Product
    {
        private float weight;
        public float Weight { get { return this.weight; } }

        public Snack(int productNumber, string name, decimal price, float weight)
        {
            this.productNumber = productNumber;
            this.name = name;
            this.price = price;
            this.weight = weight;
        }
    }
}
