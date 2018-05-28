using System.ComponentModel;
using System.Linq;
using Twittoot.Domain.Repositories;
using Twittoot.Mastodon;
using Twittoot.Twitter;
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
            
            //TODO fix this
            container.RegisterType<ISyncAccountsRepository, SyncAccountsRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITwitterService, TwitterService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMastodonService, MastodonService>(new ContainerControlledLifetimeManager());

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