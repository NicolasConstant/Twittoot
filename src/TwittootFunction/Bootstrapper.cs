using System.Linq;
using Microsoft.Practices.Unity;
using Twittoot.Domain.Sync.Repositories;
using Twittoot.Mastodon.Std;
using Twittoot.Mastodon.Std.Repositories;
using Twittoot.Twitter.Setup;
using Twittoot.Twitter.Std.Repositories;

namespace TwittootFunction
{
    public class Bootstrapper
    {
        public IUnityContainer CreateContainer(string azureTableCs, string azureTableName)
        {
            var container = new UnityContainer();

            container.RegisterType<ISyncAccountsRepository, SyncAccountsAzureTableRepository>(new ContainerControlledLifetimeManager(), new InjectionConstructor(azureTableCs, azureTableName));
            container.RegisterType<IInstancesRepository, InstancesAzureTableRepository>(
                new ContainerControlledLifetimeManager(), new InjectionConstructor(azureTableCs, azureTableName));
            container.RegisterType<ITwitterDevSettingsRepository, TwitterDevSettingsAzureTableRepository>(
                new ContainerControlledLifetimeManager(), new InjectionConstructor(azureTableCs, azureTableName));
            container.RegisterType<ITwitterUserSettingsRepository, TwitterUserSettingsAzureTableRepository>(
                new ContainerControlledLifetimeManager(), new InjectionConstructor(azureTableCs, azureTableName));

            container.RegisterType<ITwitterSyncService, TwitterSyncService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMastodonSyncService, MastodonSyncService>(new ContainerControlledLifetimeManager());

            // Register repositories
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(x => x.Name.EndsWith("Repository")),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.ContainerControlled);

            // Register services
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(x => x.Name.EndsWith("Service")),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.ContainerControlled);

            // Register facades
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(x => x.Name.EndsWith("Facade")),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.ContainerControlled);
        
            //// Register all other types
            //container.RegisterTypes(
            //    AllClasses.FromLoadedAssemblies(),
            //    WithMappings.FromMatchingInterface,
            //    WithName.Default);

            return container;
        }
    }
}