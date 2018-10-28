using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatNew
{
    //Product object. Containts Name and price
    class Product
    {
        //Product Name. Attribute and Property.  
        protected string name;
        //Product Price 
        protected decimal price;
        //Product Number
        protected int productNumber;

        public string Name { get { return this.name; }}
        public decimal Price { get { return price; } set { this.price = value; } }
        public int ProductNumber { get { return productNumber; }  }
    }
}
