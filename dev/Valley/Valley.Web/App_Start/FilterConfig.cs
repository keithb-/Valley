using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using Microsoft.Practices.ServiceLocation;

namespace Valley.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(HttpFilterCollection filters)
        {
            filters.Add(ServiceLocator.Current.GetInstance<IAuthorizationFilter>());
        }
    }
}