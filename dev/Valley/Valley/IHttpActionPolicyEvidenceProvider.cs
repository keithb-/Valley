using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http.Controllers;

namespace Valley
{
    public interface IHttpActionPolicyEvidenceProvider
    {
        IPrincipal GetUser(HttpActionContext actionContext);
        List<Claim> GetClaims(HttpActionContext actionContext);
    }
}
