using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Principal;
using System.Security.Claims;
using System.Web.Http.Controllers;

namespace Valley
{
    public class HttpHeaderEvidenceProvider : IHttpActionPolicyEvidenceProvider
    {
        //http://kevin-junghans.blogspot.com/2013/02/mixing-forms-authentication-basic.html
        public IPrincipal GetUser(HttpActionContext actionContext)
        {
            IEnumerable<string> headerVals;
            if (actionContext.Request.Headers.TryGetValues("Authorization", out headerVals))
            {
                try
                {
                    string authHeader = headerVals.FirstOrDefault();
                    char[] delims = { ' ' };
                    string[] authHeaderTokens = authHeader.Split(new char[] { ' ' });
                    if (authHeaderTokens[0].Contains("Basic"))
                    {
                        string decodedStr = DecodeFrom64(authHeaderTokens[1]);
                        string[] unpw = decodedStr.Split(new char[] { ':' });
                        return new GenericPrincipal(new GenericIdentity(unpw[0]), new string[] { });
                    }
                    else
                    {
                        if (authHeaderTokens.Length > 1)
                            return new GenericPrincipal(new GenericIdentity(DecodeFrom64(authHeaderTokens[1])), new string[] { });
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public List<Claim> GetClaims(HttpActionContext actionContext)
        {
            return new List<Claim> { new Claim("roles", "Administrator") };
        }

        private string DecodeFrom64(string encodedData)
        {

            byte[] encodedDataAsBytes
                = System.Convert.FromBase64String(encodedData);
            string returnValue =
               System.Text.Encoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;
        }
    }
}
