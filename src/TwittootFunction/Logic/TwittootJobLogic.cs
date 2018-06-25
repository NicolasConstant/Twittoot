using System.Threading.Tasks;
using Twittoot.Domain.Sync;

namespace TwittootFunction.Logic
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

        public async Task RunAsync()
        {
            await _service.RunAsync();
        }
    }
}