using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatNew
{
    class Automat
    {

        //Initializes the DAL class.
        private SnackDAL snackDAL = new SnackDAL();
        
        //List of errors to return to the user.
        private List<string> errors = new List<string>();
        //Maximum number of Products in the machine.
        private int maxNumberInMachine = 20;
        public decimal Balance { get; set; }
        public decimal ValueInMachine { get; set; }

        //List of accepted coins the machine can take.
        private List<int> acceptedCoins = new List<int>()
        {
            1,
            2,
            5,
            10,
            20
        };
        public List<int> AcceptedCoins { get { return this.acceptedCoins; } }

        //Returns a single Snack Object from the given ID.
        public Snack GetSnack(int id)
        {
            return snackDAL.GetSnack(id);
        }

        //Returns a List with all the Snack objects.
        public List<Snack> GetAllSnacks()
        {
            return snackDAL.GetAllSnacks();
        }

        //Creates new Product.
        public bool CreateNewSnack(string name, string priceString, string weightString)
        {
            decimal price;
            float weight;

            if(decimal.TryParse(priceString, out price) && float.TryParse(weightString, out weight))
            {
                if (RoomLeftInMachine())
                {
                    if (snackDAL.CreateNewSnack(name, price, weight))
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception("An error occured. The Product was not created. ");
                    }
                }
                else
                {
                    throw new Exception("An error occured. No more room in the machine.");
                }

            }
            else
            {
                throw new Exception("An error occured. The Product was not created.");

            }
        }

        //Buy Product Method
        public void BuyProduct(string userPickString)
        {
            int userPick;
            if (!int.TryParse(userPickString, out userPick) || !snackDAL.IdExist(userPick))
            {
                AddError();
                return;
            }

            //Gets the price from the selected snack
            decimal spent = snackDAL.GetSnack(userPick).Price;
            //Checks if the user can afford the item with the current balance in the machine
            if (RemoveFromBalance(spent))
            {
                //Adds the spent amount to the value in the machine, and removes one product. 
                ValueInMachine += spent;
                snackDAL.TakeOneProduct(userPick);
            }
        }

        //Checks if there is room left in the machine.
        private bool RoomLeftInMachine()
        {
            if (snackDAL.GetAllSnacks().Count < this.maxNumberInMachine)
                return true;
            else
                return false;
        }

        //Restock Product. Takes ID and Restock Amount as Parameters.
        public void RestockProduct(string productIDString, string productRestockString)
        {
            int productID;
            int productRestock;
            if (int.TryParse(productIDString, out productID) && int.TryParse(productRestockString, out productRestock))
            {
                if (productRestock > 0)
                {
                    if (snackDAL.IdExist(productID))
                    {
                        for (int i = 0; i < productRestock; i++)
                        {
                            if(RoomLeftInMachine())
                            {
                                snackDAL.CreateNewSnack(snackDAL.GetSnack(productID));
                            }
                            else
                            {
                                throw new Exception("Not enough room in machine. Only "+ i +" added.");
                            }
                            
                        }
                    }
                    else
                    {
                        AddError("ID does not exist.");
                    }
                }
                else
                {
                    AddError("Not a valid ID or Number.");
                }

            }
            else
            {
                AddError("Not a valid ID or Number.");
            }
        }

        //Change Price of Product
        public void ChangePrice(string idString, string priceString)
        {
            int id;
            decimal price;
            if (int.TryParse(idString, out id) && decimal.TryParse(priceString, out price))
            {
                if (snackDAL.IdExist(id))
                {
                    snackDAL.ChangePrice(id, price);
                }
                else
                {
                    AddError("Product ID doesn't exist.");
                }
            }
            else
            {
                AddError();
            }
        }


        //Adds to the current balance
        public void AddToBalance(int coin)
        {
            this.Balance += coin;
        }
        //Bool that returns true if the machine has a balance.
        public bool HasBalance()
        {
            if (this.Balance > 0)
                return true;
            else
                return false;
        }
        //Subtracts from the current balance
        public bool RemoveFromBalance(decimal price)
        {
            if (HasBalance())
            {
                if (Balance >= price)
                {
                    Balance = Balance - price;
                    return true;

                }
                else
                {
                    AddError("Please insert more coins before buying that Snack.");
                    return false;
                }
            }
            else
            {
                throw new Exception("No balance sat");
            }
        }
        //Resets the balance
        public void ResetBalance()
        {
            Balance = 0;
        }

        //Gives the remaining balance back. Counted in number of coins.
        public Dictionary<int, int> GetBalanceBack()
        {
            Dictionary<int, int> returnCoins = new Dictionary<int, int>();

            int balanceInt = Convert.ToInt32(Balance);
            int remainder;
            for (int i = AcceptedCoins.Count - 1; i >= 1; i--)
            {
                int numOfCoins = Math.DivRem(balanceInt, AcceptedCoins[i], out remainder);
                balanceInt = remainder;
                if (numOfCoins > 0)
                    returnCoins.Add(AcceptedCoins[i], numOfCoins);
            }
            ResetBalance();
            return returnCoins;
        }
        //Take Value
        public void TakeValue()
        {
            ValueInMachine = 0;
        }

        //User Handling
        public bool ValidateUser(string username, string password)
        {
            if(snackDAL.AdminUsers.Any(entry => entry.Key == username && entry.Value == password))
                return true;

            return false;
        }

        //Error Handling
        //Add a new error with default message. 
        //Optional Error message if you send a string as parameter
        public void AddError()
        {
            errors.Add("Not a valid option.");
        }
        public void AddError(string error)
        {
            errors.Add(error);
        }

        //Return the list of errors. 
        public List<string> GetErrors()
        {
            return errors;
        }

    }
}
