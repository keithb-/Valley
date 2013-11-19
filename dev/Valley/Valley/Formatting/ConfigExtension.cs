using System.Web.Http;

namespace Valley.Formatting
{
    public class ConfigExtension : IConfigExtension
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.Add(new GenericMediaTypeFormatter());
        }
    }
}
