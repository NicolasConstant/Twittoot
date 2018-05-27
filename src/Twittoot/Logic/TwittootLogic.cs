using System;
using System.IO;
using Twittoot.Domain;

namespace Twittoot.Logic
{
    public class TwittootLogic
    {
        private readonly TwittootService _service;

        #region Ctor
        public TwittootLogic(TwittootService service)
        {
            _service = service;
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
                    case "1":
                        RunSync();
                        break;
                    case "2":
                        AddNewAccount();
                        break;
                    case "3": 
                        ListAllAccount();
                        break;
                    case "4":
                        DeleteAccount();
                        break;
                    case "5": return;
                }
                Console.WriteLine();
            }
        }

        private void RunSync()
        {
            _service.Run();
        }

        private void AddNewAccount()
        {
            Console.WriteLine();
            Console.WriteLine("Provide Twitter Name");
            var twitterName = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Provide Mastodon Name");
            var mastodonName = Console.ReadLine();


            Console.WriteLine();
            Console.WriteLine("Provide Mastodon Instance");
            var mastodonInstance = Console.ReadLine();

            _service.RegisterNewAccount(twitterName, mastodonName, mastodonInstance);
        }

        private void ListAllAccount()
        {
            Console.WriteLine();
            var accounts = _service.GetAllAccounts();
            foreach (var syncAccount in accounts)
                Console.WriteLine($"{syncAccount.TwitterName} => {syncAccount.MastodonName}@{syncAccount.MastodonInstance}");
        }

        private void DeleteAccount()
        {
            Console.WriteLine();
            var accounts = _service.GetAllAccounts();
            for (var i = 0; i < accounts.Length; i++)
            {
                var syncAccount = accounts[i];
                Console.WriteLine($"{++i} => {syncAccount.MastodonName}@{syncAccount.MastodonInstance}");
            }

            Console.WriteLine();
            var index = -1;
            while (index == -1)
            {
                Console.WriteLine("Enter account to delete");
                var stgIndex = Console.ReadLine();
                int.TryParse(stgIndex, out index);
            }

            if (index < accounts.Length && index >= 0)
                _service.DeleteAccount(accounts[index].Id);
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