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
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Collections.ObjectModel;
using Valley.MessageHandlers;
using Valley.Models;

namespace Valley.Controllers
{
    public class ResourceController : ApiController, IDistributedHttpAuthoringController<IResource>
    {
        private readonly IResourceManager _resourceManager;
        private readonly ILockManager _lockManager;
        public ResourceController(IResourceManager resourceManager, ILockManager lockManager)
        {
            _resourceManager = resourceManager;
            _lockManager = lockManager;
        }

        [HttpOptions]
        public HttpResponseMessage Options()
        {
            if (_resourceManager.Contains(Request.RequestUri))
            {
                var resource = Get();
                if (resource is ICollection)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(DistributedHttpAuthoringControllerConstants.OptionsCollection, Encoding.Unicode, "text/plain")
                    };
                }
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(DistributedHttpAuthoringControllerConstants.OptionsResource, Encoding.Unicode, "text/plain")
                };
            }
            if (_lockManager.Contains(Request.RequestUri))
            {
                return new HttpResponseMessage(HttpStatusCode.OK) 
                {
                    Content = new StringContent(DistributedHttpAuthoringControllerConstants.OptionsLock, Encoding.Unicode, "text/plain")
                };
            }
            return new HttpResponseMessage(HttpStatusCode.NotImplemented);
        }

        [HttpHead]
        public HttpResponseMessage Head()
        {
            if (_resourceManager.Contains(Request.RequestUri))
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            if (_resourceManager.Contains(Request.RequestUri))
            {
                var resource = _resourceManager.Find(Request.RequestUri);
                var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(resource.Content) };
                response.Content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(resource.ContentType);
                response.Content.Headers.ContentLength = resource.ContentLength;
                return response;
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [HttpPost]
        public HttpResponseMessage Post(GenericContent content)
        {
            //TODO: Add media type formatters to handle both WebDAV entity 
            // as well as "pure" entity which is implemented below. A "pure"
            // entity is one where the content is posted rather than a 
            // structured (json/xml) WebDAV entity.
            var value = _resourceManager.Find(Request.RequestUri);
            if (value == null)
            {
                value = new Resource();
                value.Mappings.Add(Request.RequestUri);
            }
            value.ContentType = content.ContentType;
            value.Content = content.Body;
            _resourceManager.Save(value);

            var path = Request.RequestUri.ToString();
            var lastIndex = path.LastIndexOf('/');
            path = path.Substring(0, lastIndex);
            var collection = _resourceManager.Find(new Uri(path)) as ICollection;
            collection.Resources.Add(value);
            _resourceManager.Save(collection);
            
            var response = Request.CreateResponse(HttpStatusCode.SeeOther);
            response.Headers.Location = value.Mappings.First();
            return response;
        }

        [HttpPut]
        public HttpResponseMessage Put(GenericContent content)
        {
            var value = _resourceManager.Find(Request.RequestUri);
            if (value != null)
            {
                value.ContentType = content.ContentType;
                value.Content = content.Body;

                _resourceManager.Save(value);
            }
            else
            {
                throw new HttpResponseException(
                    Request.CreateErrorResponse(
                        HttpStatusCode.InternalServerError, 
                        new HttpError("Unable to update resource.")));
            }
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage Delete()
        {
            if (_resourceManager.Contains(Request.RequestUri))
            {
                _resourceManager.Delete(Request.RequestUri);
                Unlock();
            }
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [AcceptVerbs("PROPFIND")]
        public IPropertyValueCollection GetProperties()
        {
            //TODO: Precondition: return 403 if header Depth:(1 | infinity).

            var value = _resourceManager.Find(Request.RequestUri);
            if (value != null)
            {
                var result = new PropertyValueCollection();
                result.AddRange(value.Live);
                result.AddRange(value.Dead);
                result.AddRange(value.GetStaticProperties());
                return result;
            }
            return null;
        }

        [AcceptVerbs("PROPPATCH")]
        public HttpResponseMessage SetProperties(IPropertyValueCollection properties)
        {
            throw new NotImplementedException();
        }

        [AcceptVerbs("MKCOL")]
        public HttpResponseMessage MakeCollection()
        {
            var c = new Collection();
            c.Mappings.Add(Request.RequestUri);
            _resourceManager.Save(c);
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [AcceptVerbs("COPY")]
        public HttpResponseMessage Copy()
        {
            if (!Request.Headers.Contains(DistributedHttpRequestHeader.Destination))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            var token = Request.Headers.GetValues(DistributedHttpRequestHeader.Destination);
            foreach(var item in token) 
            {
                if (Request.RequestUri == new Uri(item))
                {
                    return new HttpResponseMessage(HttpStatusCode.Forbidden);
                }
            }
            if (_resourceManager.Contains(Request.RequestUri))
            {
                var original = _resourceManager.Find(Request.RequestUri);
                foreach (var item in token)
                {
                    var n = new Resource();
                    n.ContentType = original.ContentType;
                    n.Content = original.Content;
                    n.Mappings.Add(new Uri(item));
                    _resourceManager.Save(n);
                }
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [AcceptVerbs("MOVE")]
        public HttpResponseMessage Move()
        {
            if (!_resourceManager.Contains(Request.RequestUri))
            {
                if (Request.Headers.Contains(DistributedHttpRequestHeader.Destination))
                {
                    var token = Request.Headers.GetValues(DistributedHttpRequestHeader.Destination);
                    var original = _resourceManager.Find(Request.RequestUri);
                    var n = new Resource();
                    n.ContentType = original.ContentType;
                    n.Content = original.Content;
                    foreach (var item in token)
                    {
                        n.Mappings.Add(new Uri(item));
                    }
                    _resourceManager.Save(n);

                    _resourceManager.Delete(Request.RequestUri);
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [AcceptVerbs("LOCK")]
        public HttpResponseMessage Lock()
        {
            if (!_resourceManager.Contains(Request.RequestUri))
            {
                var k = new Resource();
                k.Mappings.Add(Request.RequestUri);
                _resourceManager.Save(k); 
            }

            //var token = new LockToken(_lockManager.BaseUri);
            var token = _lockManager.CreateToken();
            token.Resource = Request.RequestUri;
            _lockManager.Save(token);

            var response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add(DistributedHttpRequestHeader.LockToken, token.LockUri.ToString());
            return response;
        }

        [AcceptVerbs("UNLOCK")]
        public HttpResponseMessage Unlock()
        {
            if (_lockManager.Resources.Contains(Request.RequestUri))
            {
                var token = Request.Headers.GetValues(DistributedHttpRequestHeader.LockToken);
                Request.Headers.Remove(DistributedHttpRequestHeader.LockToken);
                foreach(var item in token)
                {
                    _lockManager.Delete(new Uri(item));
                }
            }
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
