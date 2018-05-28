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
        static void Main(string[] args)
        {
            var container = GetContainer();
            var program = container.Resolve<Program>();
            program.Run(args);
        }

        private static IUnityContainer GetContainer()
        {
            var bootstrapper = new Bootstrapper();
            return bootstrapper.CreateContainer();
        }
    }

    class Program
    {
        private readonly IntroDisplay _introDisplay;
        private readonly TwittootLogic _logic;

        #region Ctor
        public Program(IntroDisplay introDisplay, TwittootLogic logic)
        {
            _introDisplay = introDisplay;
            _logic = logic;
        }
        #endregion

        public void Run(string[] args)
        {
            try
            {
                _introDisplay.Run();
                _logic.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
