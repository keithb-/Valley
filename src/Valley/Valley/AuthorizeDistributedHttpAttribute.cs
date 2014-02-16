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
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;

namespace Valley
{
    //http://stackoverflow.com/questions/6121050/mvc-3-unity-2-inject-dependencies-into-a-filter
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizeDistributedHttpAttribute : AuthorizationFilterAttribute
    {
        private IHttpActionPolicyEnforcer _policyEnforcer;
        private IHttpActionPolicyEvidenceProvider _evidenceProvider;

        public AuthorizeDistributedHttpAttribute() {}

        public AuthorizeDistributedHttpAttribute(IHttpActionPolicyEvidenceProvider evidenceProvider, IHttpActionPolicyEnforcer policyEnforcer)
        {
            _policyEnforcer = policyEnforcer;
            _evidenceProvider = evidenceProvider;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (_policyEnforcer != null)
            {
                if (!_policyEnforcer.IsAuthorized(
                    _evidenceProvider.GetClaims(actionContext),
                    _evidenceProvider.GetUser(actionContext),
                    new[] { actionContext.Request.Method.Method },
                    actionContext.Request.RequestUri.ToString()))
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
                }
                return;
            }
            base.OnAuthorization(actionContext);
        }
    }
}
