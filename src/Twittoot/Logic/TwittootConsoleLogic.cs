using System;
using System.IO;
using System.Threading.Tasks;
using Twittoot.Domain;
using Twittoot.Domain.Sync;

namespace Twittoot.Logic
{
    public class TwittootConsoleLogic
    {
        private readonly ITwittootSetupFacade _setupService;
        private readonly ITwittootSyncFacade _syncService;

        #region Ctor
        public TwittootConsoleLogic(ITwittootSetupFacade setupService, ITwittootSyncFacade syncService)
        {
            _setupService = setupService;
            _syncService = syncService;
        }
        #endregion

        public async Task RunAsync()
        {
            for (;;)
            {
                DisplayMenu();
                var result = Console.ReadLine();
                switch (result)
                {
                    case "1":
                        await RunSync();
                        break;
                    case "2":
                        await AddNewAccount();
                        break;
                    case "3": 
                        await ListAllAccount();
                        break;
                    case "4":
                        await DeleteAccount();
                        break;
                    case "5": return;
                }
                Console.WriteLine();
            }
        }

        private async Task RunSync()
        {
            await _syncService.RunAsync();
        }

        private async Task AddNewAccount()
        {
            await _setupService.RegisterNewAccountAsync();
        }

        private async Task ListAllAccount()
        {
            Console.WriteLine();
            var accounts = await _setupService.GetAllAccounts();
            foreach (var syncAccount in accounts)
                Console.WriteLine($"{syncAccount.TwitterName} => {syncAccount.MastodonName}@{syncAccount.MastodonInstance}");
        }

        private async Task DeleteAccount()
        {
            Console.WriteLine();
            var accounts = await _setupService.GetAllAccounts();
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

            if (index <= accounts.Length && index > 0)
                await _setupService.DeleteAccount(accounts[--index].Id);
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