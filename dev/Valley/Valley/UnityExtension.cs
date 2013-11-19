using System.Linq;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;

namespace Valley
{
    public class UnityExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.AddNewExtension<Valley.MessageHandlers.UnityExtension>();

            // Register the constructor with arguments.
            this.Container.RegisterType<IAuthorizationFilter, AuthorizeDistributedHttpAttribute>(
                new ContainerControlledLifetimeManager(), 
                new InjectionConstructor(
                    new ResolvedParameter(typeof(IHttpActionPolicyEvidenceProvider)), 
                    new ResolvedParameter(typeof(IHttpActionPolicyEnforcer))));
        }
    }
}
