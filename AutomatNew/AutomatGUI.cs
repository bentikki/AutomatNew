using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatNew
{
    //GUI class.
    class AutomatGUI
    {
        //Controller class Initialized
        private Automat automat = new Automat();
        //Exit bool. Used to check if the application should repeat
        private bool exit;

        //Displays menu in Main. Repeats while exit is not set to true.
        public void DisplayMenu()
        {
            do
            {
                try
                {
                    Console.Clear();
                    DisplayErrors();
                    DisplayMainMenu();
                }
                catch (Exception e)
                {
                    //Displays Error message.
                    Console.WriteLine("Error:");
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }

            } while (!exit);
        }
        
        //Prints out the main menu.
        private void DisplayMainMenu()
        {
            Console.WriteLine("---Snack and Drink Dispenser---");
            //Displays the snack grid
            DisplaySnacks();
            Console.WriteLine("1. Choose Product");
            Console.WriteLine("2. Insert Coins");
            Console.WriteLine("3. Get remaining balance back");
            Console.WriteLine("4. Admin");
            Console.WriteLine("5. Exit Console");
            //Displays current balance, if it exists.
            CurrentBalance();
            string userInput = Console.ReadLine();
            MainMenuControl(userInput);
        }

        /*
        Main Menu Control. Switch case and Input check. 
        Calls the method based on user choice
        */
        private void MainMenuControl(string userString)
        {
            int input;
            if (int.TryParse(userString, out input))
            {
                switch (input)
                {
                    case 1:
                        ChooseProductMenu();
                        break;
                    case 2:
                        DisplayCoinMenu();
                        break;
                    case 3:
                        GetBalanceBack();
                        break;
                    case 4:
                        DisplayAdminMenu();
                        break;
                    case 5:
                        ExitMenu();
                        break;
                    default:
                        automat.AddError();
                        break;
                }
            }
            else
            {
                automat.AddError();
            }
        }

        //Displays the Login request for the admin menu.
        private void DisplayAdminMenu()
        {
            Console.WriteLine("Admin Username:");
            string username = Console.ReadLine();
            Console.WriteLine("Admin Password:");
            string password = Console.ReadLine();

            if(automat.ValidateUser(username, password))
            {
                AdminMenu();
            }
            else
            {
                automat.AddError("No User Found");
            }
        }

        //Prints out the Admin Menu.
        private void AdminMenu()
        {
            //Exit key sets the key press needed to return to Main Menu.
            //Repeat determines whether the menu should repeat or exit. 
            int exitKey = 5;
            bool repeat = true;
            do
            {
                Console.Clear();
                DisplayErrors();

                Console.WriteLine("Admin menu");

                //Displays the snack grid.
                DisplaySnacks();

                Console.WriteLine("1. Restock Product");
                Console.WriteLine("2. Add new Product");
                Console.WriteLine("3. Change Price of Product");
                Console.WriteLine("4. Take Value (Current Value = {0})", automat.ValueInMachine);
                Console.WriteLine("{0}. Return to Main Menu", exitKey);
                string userInputString = Console.ReadLine();


                int userInput;
                if (int.TryParse(userInputString, out userInput))
                {
                    //Checks if the Input key is the exit key sat. 
                    //Stops the repeat if true.
                    if(userInput == exitKey)
                    {
                        repeat = false;
                    }
                    else
                    {
                        AdminMenuControl(userInput);
                    }
                }
                else
                {
                    automat.AddError();
                }
                

            } while (repeat);
        }

        //Switch case of the admin menu. Based on the int given as parameter.
        private void AdminMenuControl(int userInput)
        {
            switch (userInput)
            {
                case 1:
                    Console.WriteLine("Barcode: ");
                    string productID = Console.ReadLine();
                    Console.WriteLine("Number of Restock: ");
                    string productRestock = Console.ReadLine();

                    automat.RestockProduct(productID, productRestock);

                    break;
                case 2:
                    Console.WriteLine("Snack Name: ");
                    string snackName = Console.ReadLine();
                    Console.WriteLine("Snack Price: ");
                    string snackPrice = Console.ReadLine();
                    Console.WriteLine("Snack Weight in mg: ");
                    string snackWeight = Console.ReadLine();

                    if(automat.CreateNewSnack(snackName, snackPrice, snackWeight))
                    {
                        Console.WriteLine("Product Created!");
                        Console.ReadKey();
                    }
                   
                    break;
                case 3:
                    Console.WriteLine("Barcode: ");
                    string newPriceID = Console.ReadLine();
                    Console.WriteLine("New Price: ");
                    string newPrice = Console.ReadLine();

                    automat.ChangePrice(newPriceID, newPrice);

                    break;
                case 4:
                    automat.TakeValue();
                    break;
                default:
                    automat.AddError();
                    break;
            }
        }


        //Get remaining balance back Menu
        private void GetBalanceBack()
        {
            if (automat.HasBalance())
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;

                Console.WriteLine("You get back {0} Dkk total.", automat.Balance);

                foreach (var coin in automat.GetBalanceBack())
                {
                    Console.WriteLine("You get {0}x {1} coin back.", coin.Value, coin.Key);
                }

                Console.ResetColor();
                Console.WriteLine("Continue...");
                Console.ReadKey();
            }
            else
            {
                automat.AddError("You have no active balance.");
                return;
            }   
        }

        //Choose Product Menu
        private void ChooseProductMenu()
        {
            if (automat.HasBalance())
            {
                Console.Write("Write Product ID:");
                string userProductPick = Console.ReadLine();

                automat.BuyProduct(userProductPick);
            }
            else
            {
                automat.AddError("There are no coins in the machine.");
            }
        }

        //Display Coin Menu
        private void DisplayCoinMenu()
        {
            bool repeat = true;
            do
            {
                Console.Clear();
                DisplayErrors();

                Console.WriteLine("Insert Coins");
                int menuOption = 1;
                for (int i = 0; i < automat.AcceptedCoins.Count; i++)
                {
                    Console.WriteLine("{0}. {1} kr", menuOption, automat.AcceptedCoins[i]);
                    menuOption++;
                }
                int exitOption = menuOption;
                Console.WriteLine("{0}. Back to main menu", exitOption);

                CurrentBalance();

                int userCoinPick;
                string userCoinPickString = Console.ReadLine();

                if (int.TryParse(userCoinPickString, out userCoinPick))
                {
                    if (userCoinPick == exitOption)
                    {
                        repeat = false;
                    }
                    else if (userCoinPick <= automat.AcceptedCoins.Count)
                    {
                        int addAmount = automat.AcceptedCoins[userCoinPick - 1];
                        automat.AddToBalance(addAmount);
                    }
                    else
                    {
                        automat.AddError();
                    }
                }
                else
                {
                    automat.AddError();
                }
            } while (repeat);

        }

        //Exit Menu
        private void ExitMenu()
        {
            exit = true;
        }

        //Displays Current Balance if it exists
        private void CurrentBalance()
        {
            if (automat.HasBalance())
            {
                Console.WriteLine();
                Console.WriteLine("Current balance {0} kr", automat.Balance);
            }
        }

        //Displays the snack list.
        private void DisplaySnacks()
        {
            Console.WriteLine();
            foreach (var snack in automat.GetAllSnacks())
            {
                DisplayMultiple(snack);
            }
            Console.WriteLine();
        }

        //Prints out the given Snack Object.
        private void DisplayMultiple(Snack snack)
        {
            //Console.WriteLine("Barcode: " + snack.ProductNumber);
            //Console.WriteLine("Snack Name: " + snack.Name);
            //Console.WriteLine("Weight: " + snack.Weight + " mg");
            //Console.WriteLine("Price: " + snack.Price + " Dkk");
            //Console.WriteLine();

            string displaystring = string.Format("Barcode: {0,-5} Snack Name: {1,-20} Weight: {2,-10} Price: {3}", snack.ProductNumber, snack.Name, (snack.Weight+"mg"), (snack.Price+".- Dkk"));

            Console.WriteLine(displaystring);
        }

        //Displays the errors.
        private void DisplayErrors()
        {
            for (int i = 0; i < automat.GetErrors().Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error: " + automat.GetErrors()[i]);
                Console.ResetColor();
            }
            automat.GetErrors().Clear();
        }


    }
}
