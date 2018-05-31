using System;
using Twittoot.Domain;

namespace Twittoot.Logic
{
    public class TwittootJobLogic
    {
        private readonly ITwittootFacade _service;

        #region Ctor
        public TwittootJobLogic(ITwittootFacade service)
        {
            _service = service;
        }
        #endregion

        public void Run()
        {
            _service.Run();
        }
    }
}