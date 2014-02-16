/*
   Copyright 2014 Keith R. Bielaczyc

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace Valley.Web
{
    //http://www.devtrends.co.uk/blog/introducing-the-unity.mvc3-nuget-package-to-reconcile-mvc3-unity-and-idisposable
    //http://stackoverflow.com/questions/9527988/cannot-inject-dependencies-into-asp-net-web-api-controller-using-unity
    //http://www.strathweb.com/2012/11/asp-net-web-api-and-dependencies-in-request-scope/
    //http://www.asp.net/web-api/overview/extensibility/using-the-web-api-dependency-resolver
    public static class Bootstrapper
    {
        public static IUnityContainer Initialize()
        {
            var container = new UnityContainer();

            //TODO: Load these values from configuration.
            var baseUri = new Uri("http://localhost/");
            var lockBaseUri = new Uri("http://localhost/system/lock/");

            container.RegisterType<Valley.IResourceManager, Valley.Storage.ResourceManager>(new ContainerControlledLifetimeManager(), new InjectionConstructor(baseUri));
            container.RegisterType<Valley.ILockManager, Valley.Storage.LockManager>(new ContainerControlledLifetimeManager(), new InjectionConstructor(lockBaseUri));
            container.RegisterType<Valley.IHttpActionPolicyEvidenceProvider, MockEvidenceProvider>(new ContainerControlledLifetimeManager());
            container.RegisterType<Valley.IHttpActionPolicyEnforcer, MockPolicyEnforcer>(new ContainerControlledLifetimeManager());

            container.AddNewExtension<Valley.UnityExtension>();

            var resolver = new Unity.WebApi.UnityDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

            var locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);

            return container;
        }
    }

    class MockEvidenceProvider : IHttpActionPolicyEvidenceProvider
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

    class MockPolicyEnforcer : IHttpActionPolicyEnforcer
    {
        public bool IsAuthorized(
            System.Collections.Generic.List<System.Security.Claims.Claim> claims, 
            System.Security.Principal.IPrincipal userPrincipal, 
            string[] actions, 
            string resourceUri)
        {
            claims.ForEach(c => System.Diagnostics.Debug.WriteLine(c.Type + " " + c.Value));
            if ((userPrincipal != null) && (userPrincipal.Identity != null))
            {
                System.Diagnostics.Debug.WriteLine(userPrincipal.Identity.Name);
            }
            System.Diagnostics.Debug.WriteLine(string.Join(", ", actions));
            System.Diagnostics.Debug.WriteLine(resourceUri);
            return !actions.Any(a => ((a == "DELETE") || (a == "MOVE") || (a == "PROPPATCH")));
        }
    }
}