using System.IO;
using mastodon.Models;
using Newtonsoft.Json;
using Twittoot.Common;
using Twittoot.Mastodon.Models;

namespace Twittoot.Mastodon.Repositories
{
    public interface IInstancesRepository
    {
        AppInfoWrapper[] GetAllInstances();
        void SaveInstances(AppInfoWrapper[] instances);
    }

    public class InstancesRepository : IInstancesRepository
    {
        private const string InstancesFileName = "Instances";
        private readonly string _instancesFilePath = TwittootLocation.GetUserFilePath(InstancesFileName);

        #region Ctor
        public InstancesRepository()
        {
            if (!File.Exists(_instancesFilePath)) File.Create(_instancesFilePath);
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