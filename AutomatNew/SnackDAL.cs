using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatNew
{
    class SnackDAL
    {
        //List of current Snack objects with test data.
        private List<Snack> snacks = new List<Snack>();

        //Contains the admin users, including a username and password for each Admin.
        private Dictionary<string, string> adminUsers = new Dictionary<string, string>();

        public Dictionary<string, string> AdminUsers { get { return this.adminUsers; } }

        //Constructor. Simply used to create admin users.
        public SnackDAL()
        {
            Snack bounty = new Snack(1,"Bounty Bar",10,20);
            this.snacks.Add(bounty);
            Snack twix =  new Snack(2, "Twix", 15, 33);
            this.snacks.Add(twix);


            adminUsers.Add("admin", "1234");
        }

        //Returns all Snacks, ordered by product number
        public List<Snack> GetAllSnacks()
        {
            return snacks.OrderBy(snack => snack.ProductNumber).ToList();
        }

        //Create a new Snack
        public bool CreateNewSnack(string name, decimal price, float weight)
        {
            //Get distinct ID
            int id = GetAllSnacks().Select(snack => snack.ProductNumber).Distinct().Reverse().First() + 1;
            Snack newSnack = new Snack(id, name, price, weight);
            snacks.Add(newSnack);
            return true;
        }
        public bool CreateNewSnack(Snack snack)
        {
            Snack newSnack = new Snack(snack.ProductNumber, snack.Name, snack.Price, snack.Weight);
            snacks.Add(newSnack);
            return true;
        }

        //Changes the price of the Snack with the given ID. Sets it to the given decimal price
        public void ChangePrice(int id, decimal price)
        {
            foreach (Snack snack in GetAllSnacks())
            {
                if (snack.ProductNumber == id)
                    snack.Price = price;
            }
        }

        //Returns the Snack with the given ID
        public Snack GetSnack(int id)
        {
            if (IdExist(id))
            {
                return snacks.Find(snack => snack.ProductNumber == id);
            }
            else
            {
                throw new Exception("Product ID does not exist. From GetSnack");
            }
            
        }

        //Checks if the given ID exists. Returns true if ID exists.
        public bool IdExist(int id)
        {
            if (snacks.Any(snack => snack.ProductNumber == id))
                return true;

            return false;
        }

        //Subtracts 1 from product.
        public bool TakeOneProduct(int id)
        {
            Snack snackObj = GetSnack(id);

            if (RemoveSnack(snackObj))
            {
                return true;
            }
            return false;
        }

        //Removes a snack object.
        private bool RemoveSnack(Snack snackObj)
        {
            if (!this.snacks.Remove(snackObj))
            {
                throw new Exception("Could not remove snack from list.");
            }
            return true;
        }

    }
}
