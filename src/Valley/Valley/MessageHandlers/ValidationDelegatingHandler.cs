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
using System.Net.Http.Headers;

namespace Valley.MessageHandlers
{
    public class ValidationDelegatingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken)
                .ContinueWith<HttpResponseMessage>(
                (responseToCompleteTask) => {

                    //return request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError("Validation failed."));

                    var response = responseToCompleteTask.Result;
                    //TODO: Check the request for known errors.
                    return response;
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private bool TryNoExternalEntities(HttpRequestMessage request, ref HttpResponseMessage error)
        {
            if (!request.Headers.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/xml")))
            {
                return true;
            }

            //TODO: Check document for entities.
            var content = request.Content.ReadAsStringAsync().Result;

            if (!content.Contains("some entity"))
            {
                //TODO: return XML according to the spec.
                error = request.CreateErrorResponse(HttpStatusCode.Forbidden,
                    new HttpError("[RFC 4918] no-external-entities: If the server rejects a client request " +
                                  "because the request body contains an external entity, the server " +
                                  "SHOULD use this error."));
                return false;
            }
            return true;
        }
    }
}