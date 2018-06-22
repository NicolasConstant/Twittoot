using System.IO;
using Newtonsoft.Json;
using Twittoot.Common;
using Twittoot.Mastodon.Std.Models;

namespace Twittoot.Mastodon.Std.Repositories
{
    public interface IInstancesRepository
    {
        AppInfoWrapper[] GetAllInstances();
        void SaveInstances(AppInfoWrapper[] instances);
    }

    public class InstancesRepository : IInstancesRepository
    {
        private const string InstancesFileName = "Instances.json";
        private readonly string _instancesFilePath = TwittootLocation.GetUserFilePath(InstancesFileName);

        #region Ctor
        public InstancesRepository()
        {
            var json = JsonConvert.SerializeObject(new AppInfoWrapper[0]);
            if (!File.Exists(_instancesFilePath)) File.WriteAllText(_instancesFilePath, json);
        }
        #endregion

        public AppInfoWrapper[] GetAllInstances()
        {
            var json = File.ReadAllText(_instancesFilePath);
            return JsonConvert.DeserializeObject<AppInfoWrapper[]>(json);
        }

        public void SaveInstances(AppInfoWrapper[] instances)
        {
            var json = JsonConvert.SerializeObject(instances);
            File.WriteAllText(_instancesFilePath, json);
        }
    }
}