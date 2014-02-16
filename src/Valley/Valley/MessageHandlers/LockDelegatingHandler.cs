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
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Web.Http;
using System.Web;
using System.Collections.Generic;
using Valley.Models;

namespace Valley.MessageHandlers
{
    //http://www.asp.net/web-api/overview/working-with-http/http-message-handlers
    public class LockDelegatingHandler : DelegatingHandler
    {
        private ILockManager _lockManager;

        public LockDelegatingHandler(ILockManager lockManager)
        {
            _lockManager = lockManager;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_lockManager == null)
            {
                //http://www.strathweb.com/2012/11/asp-net-web-api-and-dependencies-in-request-scope/
                _lockManager = request.GetDependencyScope().GetService(typeof(ILockManager)) as ILockManager;
            }
            // Preconditions
            HttpResponseMessage error;
            if (!TryNoLockTokenMatchesRequestUri(request, out error))
            {
                return base.SendAsync(request, cancellationToken)
                    .ContinueWith<HttpResponseMessage>((responseToCompleteTask) => { return error; });
            }

            if (!TryNoLockTokenSubmitted(request, out error))
            {
                return base.SendAsync(request, cancellationToken)
                    .ContinueWith<HttpResponseMessage>((responseToCompleteTask) => { return error; });
            }

            if (!TryNoConflictingLock(request, out error))
            {
                return base.SendAsync(request, cancellationToken)
                    .ContinueWith<HttpResponseMessage>((responseToCompleteTask) => { return error; });
            }
            return base.SendAsync(request, cancellationToken);
        }

        private bool TryNoLockTokenMatchesRequestUri(HttpRequestMessage request, out HttpResponseMessage error)
        {
            error = null;
            if (request.Method.Equals(DistributedHttpMethod.Unlock))
            {
                if (request.Headers.Contains(DistributedHttpRequestHeader.LockToken))
                {
                    var lockTokenList = request.Headers.GetValues(DistributedHttpRequestHeader.LockToken);
                    var lockToken = lockTokenList.GetEnumerator();
                    if (lockToken.MoveNext())
                    {
                        if (!_lockManager.Contains(new Uri(lockToken.Current)))
                        {
                            //TODO: return XML according to the spec.
                            error = request.CreateErrorResponse(HttpStatusCode.Conflict,
                                new HttpError("[RFC 4918] lock-token-matches-request-uri: The lock may have " +
                                              "a scope that does not include the Request-URI, or the lock could " +
                                              "have disappeared, or the token may be invalid."));
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool TryNoLockTokenSubmitted(HttpRequestMessage request, out HttpResponseMessage error)
        {
            error = null;
            var methods = new List<HttpMethod> { 
                HttpMethod.Put, 
                HttpMethod.Post, 
                DistributedHttpMethod.PropertyPatch, 
                DistributedHttpMethod.Unlock, 
                DistributedHttpMethod.Move,
                DistributedHttpMethod.Copy,
                HttpMethod.Delete, 
                DistributedHttpMethod.MakeCollection };
            if (methods.Contains(request.Method))
            {
                if (!request.Headers.Contains(DistributedHttpRequestHeader.LockToken))
                {
                    //TODO: return XML according to the spec.
                    error = request.CreateErrorResponse((HttpStatusCode)DistributedHttpStatusCode.Locked.Value,
                        new HttpError("[RFC 4918] lock-token-submitted: The request could not succeed because " +
                                      "a lock token should have been submitted."));
                    return false;
                }
                var token = request.Headers.GetValues(DistributedHttpRequestHeader.LockToken);
                foreach (var item in token)
                {
                    if (!_lockManager.Contains(new Uri(item)))
                    {
                        //TODO: return XML according to the spec.
                        error = request.CreateErrorResponse((HttpStatusCode)DistributedHttpStatusCode.Locked.Value,
                            new HttpError("[RFC 4918] lock-token-submitted: The request could not succeed because " +
                                          "a lock token should have been submitted."));
                        return false;
                    }
                }
                if (!_lockManager.Resources.Contains(request.RequestUri))
                {
                    //TODO: return XML according to the spec.
                    error = request.CreateErrorResponse((HttpStatusCode)DistributedHttpStatusCode.Locked.Value,
                        new HttpError("[RFC 4918] lock-token-submitted: The request could not succeed because " +
                                      "a lock token should have been submitted."));
                    return false;
                }
            }
            return true;
        }

        private bool TryNoConflictingLock(HttpRequestMessage request, out HttpResponseMessage error)
        {
            error = null;
            if (request.Method.Equals(DistributedHttpMethod.Lock))
            {
                if (_lockManager.Resources.Contains(request.RequestUri))
                {
                    //TODO: return XML according to the spec.
                    error = request.CreateErrorResponse((HttpStatusCode)DistributedHttpStatusCode.Locked.Value,
                        new HttpError("[RFC 4918] no-conflicting-lock: A LOCK request failed due the presence " +
                                      "of an already existing conflicting lock."));
                    return false;
                }
            }
            return true;
        }
    }
}