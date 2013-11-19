using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Valley
{
    public static class ConfigExtension
    {
        public static void Register(HttpConfiguration config)
        {
            var type = typeof(IConfigExtension);
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);            
            foreach(var c in types)
            {
                var method = c.GetMethod("Register");
                method.Invoke(null, new [] { config });            
            }
        }
    }
}
