using System.ComponentModel;
using System.Linq;
using Twittoot.Domain;
using Twittoot.Domain.Sync.Repositories;
using Twittoot.Mastodon;
using Twittoot.Mastodon.Setup;
using Twittoot.Mastodon.Std;
using Twittoot.Mastodon.Std.Repositories;
using Twittoot.Twitter;
using Twittoot.Twitter.Setup;
using Twittoot.Twitter.Std.Repositories;
using Unity;
using Unity.Lifetime;
using Unity.RegistrationByConvention;

namespace Twittoot
{
    public class Bootstrapper
    {
        public IUnityContainer CreateContainer()
        {
            var container = new UnityContainer();
            
            //File system bootstrap
            container.RegisterType<ISyncAccountsRepository, SyncAccountsFileRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IInstancesRepository, InstancesFileRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITwitterDevSettingsRepository, TwitterDevSettingsFileRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITwitterUserSettingsRepository, TwitterUserSettingsFileRepository>(new ContainerControlledLifetimeManager());
            

            container.RegisterType<ITwitterSetupService, TwitterSetupService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITwitterSyncService, TwitterSyncService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMastodonSetupService, MastodonSetupService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMastodonSyncService, MastodonSyncService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITwittootSetupFacade, TwittootSetupFacade>(new ContainerControlledLifetimeManager());
            
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

            // Register all other types
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies(),
                WithMappings.FromMatchingInterface,
                WithName.Default);

            return container;
        }
    }
}