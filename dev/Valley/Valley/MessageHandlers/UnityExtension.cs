using Microsoft.Practices.Unity;

namespace Valley.MessageHandlers
{
    public class UnityExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterType<ValidationDelegatingHandler>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<LockDelegatingHandler>(new ContainerControlledLifetimeManager(), new InjectionConstructor(new ResolvedParameter(typeof(ILockManager))));
        }
    }
}