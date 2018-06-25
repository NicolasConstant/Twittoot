using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Practices.Unity;
using TwittootFunction.Logic;

namespace TwittootFunction
{
    public static class TwitterMastodonSync
    {
        [FunctionName("TwitterMastodonSync")]
        public static async Task Run([TimerTrigger("0 */15 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"TwitterMastodonSync started at: {DateTime.Now}");

            try
            {
                var azureTableCs = GetEnvironmentVariable("AzureTableCs");
                var azureTableName = GetEnvironmentVariable("AzureTableName");
                var bootstrapper = new Bootstrapper();
                var container = bootstrapper.CreateContainer(azureTableCs, azureTableName);
                var program = container.Resolve<TwittootJobLogic>();
                await program.RunAsync();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }

            log.Info($"TwitterMastodonSync ended at: {DateTime.Now}");
        }

        public static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
