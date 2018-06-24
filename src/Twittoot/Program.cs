using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Twittoot.Domain;
using Twittoot.Logic;
using Twittoot.Mastodon;
using Twittoot.Twitter;
using Unity;

namespace Twittoot
{
    static class ProgramEntry
    {
        [STAThread]
        static void Main(string[] args)
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            IUnityContainer container;
            if (args.Contains("azure"))
            {
                var azureCs = ConfigurationManager.AppSettings["AzureCs"];
                var azureTable = ConfigurationManager.AppSettings["AzureTable"];
                container = GetContainer(true, azureCs, azureTable);
            }
            else
            {
                container = GetContainer(false, string.Empty, string.Empty);
            }


            if (args.Contains("setup"))
            {
                var program = container.Resolve<ConsoleProgram>();
                var t = program.RunAsync();
                t.Wait();
            }
            else
            {
                var program = container.Resolve<JobProgram>();
                var t = program.RunAsync();
                t.Wait();
            }
        }

        private static IUnityContainer GetContainer(bool useAzuerTable, string azureTableCs, string azureTableName)
        {
            var bootstrapper = new Bootstrapper();
            return bootstrapper.CreateContainer(useAzuerTable, azureTableCs, azureTableName);
        }
    }

    class ConsoleProgram
    {
        private readonly IntroDisplay _introDisplay;
        private readonly TwittootConsoleLogic _logic;

        #region Ctor
        public ConsoleProgram(IntroDisplay introDisplay, TwittootConsoleLogic logic)
        {
            _introDisplay = introDisplay;
            _logic = logic;
        }
        #endregion

        public async Task RunAsync()
        {
            try
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                _introDisplay.Run();
                await _logic.RunAsync();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }
    }

    class JobProgram
    {
        private readonly TwittootJobLogic _logic;

        #region Ctor
        public JobProgram(TwittootJobLogic logic)
        {
            _logic = logic;
        }
        #endregion

        public async Task RunAsync()
        {
            try
            {
                await _logic.Run();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
            }
        }
    }
}
