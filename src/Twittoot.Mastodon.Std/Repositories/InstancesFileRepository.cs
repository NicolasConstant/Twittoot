using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Twittoot.Common;
using Twittoot.Mastodon.Std.Models;

namespace Twittoot.Mastodon.Std.Repositories
{
    public class InstancesFileRepository : IInstancesRepository
    {
        private const string InstancesFileName = "Instances.json";
        private readonly string _instancesFilePath = TwittootLocation.GetUserFilePath(InstancesFileName);

        #region Ctor
        public InstancesFileRepository()
        {
            var json = JsonConvert.SerializeObject(new AppInfoWrapper[0]);
            if (!File.Exists(_instancesFilePath)) File.WriteAllText(_instancesFilePath, json);
        }
        #endregion

        public async Task<AppInfoWrapper[]> GetAllInstancesAsync()
        {
            var json = File.ReadAllText(_instancesFilePath);
            return JsonConvert.DeserializeObject<AppInfoWrapper[]>(json);
        }

        public async Task SaveInstancesAsync(AppInfoWrapper[] instances)
        {
            var json = JsonConvert.SerializeObject(instances);
            File.WriteAllText(_instancesFilePath, json);
        }
    }
}