using System.Web.Http;

namespace Valley.Controllers
{
    public class ConfigExtension : IConfigExtension
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "Lock",
                routeTemplate: "system/lock/{id}",
                defaults: new { controller = "Lock", id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "Resource",
                routeTemplate: "{*url}",
                defaults: new { controller = "Resource" }
            );
        }
    }
}