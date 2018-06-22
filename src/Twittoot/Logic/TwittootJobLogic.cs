using System;
using System.Threading.Tasks;
using Twittoot.Domain;
using Twittoot.Domain.Sync;

namespace Twittoot.Logic
{
    public class TwittootJobLogic
    {
        private readonly ITwittootSyncFacade _service;

        #region Ctor
        public TwittootJobLogic(ITwittootSyncFacade service)
        {
            _service = service;
        }
        #endregion

        public async Task Run()
        {
            await _service.RunAsync();
        }
    }
}