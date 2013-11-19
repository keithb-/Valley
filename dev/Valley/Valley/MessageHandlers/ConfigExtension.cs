using System.Net.Http;
using System.Web.Http;

namespace Valley.MessageHandlers
{
    public class ConfigExtension : IConfigExtension
    {
        public static void Register(HttpConfiguration config)
        {
            config.MessageHandlers.Add((DelegatingHandler)config.DependencyResolver.GetService(typeof(ValidationDelegatingHandler)));
            config.MessageHandlers.Add((DelegatingHandler)config.DependencyResolver.GetService(typeof(LockDelegatingHandler)));
        }
    }
}