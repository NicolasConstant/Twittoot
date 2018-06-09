using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Twittoot.Domain;
using Twittoot.Domain.Repositories;
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
            var container = GetContainer();

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

        private static IUnityContainer GetContainer()
        {
            var bootstrapper = new Bootstrapper();
            return bootstrapper.CreateContainer();
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
