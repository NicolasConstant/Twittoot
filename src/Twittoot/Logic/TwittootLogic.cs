using System;
using System.IO;

namespace Twittoot.Logic
{
    public class TwittootLogic
    {
        #region Ctor
        public TwittootLogic()
        {
            
        }
        #endregion

        public void Run()
        {
            for (;;)
            {
                DisplayMenu();
                var result = Console.ReadLine();
                switch (result)
                {
                    case "1": break;
                    case "2": break;
                    case "3": break;
                    case "4": break;
                    case "5": return;
                }
                Console.WriteLine();
            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine("Please select action");
            Console.WriteLine();
            Console.WriteLine("1. Run Sync");
            Console.WriteLine("2. Add new account");
            Console.WriteLine("3. List all accounts");
            Console.WriteLine("4. Delete account");
            Console.WriteLine("5. Quit");
            Console.WriteLine();
        }
    }
}