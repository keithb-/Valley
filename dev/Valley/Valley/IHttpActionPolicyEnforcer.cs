using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Valley
{
    public interface IHttpActionPolicyEnforcer
    {
        bool IsAuthorized(List<Claim> claims, IPrincipal userPrincipal, string[] actions, string resourceUri);
    }
}
