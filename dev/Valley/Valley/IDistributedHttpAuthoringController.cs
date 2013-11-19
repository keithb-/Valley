using System.Net.Http;
using System.Web.Mvc;
using Valley.Models;

namespace Valley
{
    public sealed class DistributedHttpAuthoringControllerConstants
    {
        public const string OptionsCollection = "COPY,DELETE,GET,HEAD,LOCK,MKCOL,MOVE,OPTIONS,POST,PROPFIND,PROPPATCH,PUT,UNLOCK";
        public const string OptionsResource = "COPY,DELETE,GET,HEAD,LOCK,MOVE,OPTIONS,POST,PROPFIND,PROPPATCH,PUT,UNLOCK";
        public const string OptionsLock = "GET,HEAD,OPTIONS,PROPFIND";
    }
    //kbielaczyc.2013.08.16: Attributes on this interface are not added
    //to the routing table. Probably needs a route mapping class in order
    //to walk up to these interface declarations. Copying all attributes
    //to the controller classes.
    //kbielaczyc.2013.09.26: Based on the "ASP.NET Web API" poster, I'm 
    //going with the assumption that a "void" return type is really 
    //for special cases where the response body will be empty. For all
    //other processing, the return value should either be HttpResponseMessage
    //or some negotiated content which most likely means 200 OK with a 
    //response body. Obviously, I can still return an HttpResponseMessage(204)
    //when necessary or in those cases where I have already indicated that I
    //will return an HttpResponseMessage.
    public interface IDistributedHttpAuthoringController<T>
    {
        [HttpOptions]
        HttpResponseMessage Options();

        [HttpHead]
        HttpResponseMessage Head();

        //kbielaczyc.2013.08.06: if the url is a valid resource,
        //then having an overload with an automatic binding
        //doesn't make any sense.
        //[HttpHead]
        //HttpResponseMessage Head(string id);

        [HttpGet]
        T Get();
        
        [HttpPost]
        HttpResponseMessage Post(GenericContent content);
        
        [HttpPut]
        HttpResponseMessage Put(GenericContent content);
        
        [HttpDelete]
        HttpResponseMessage Delete();
        
        [AcceptVerbs("PROPFIND")]
        IPropertyValueCollection GetProperties();

        [AcceptVerbs("PROPPATCH")]
        HttpResponseMessage SetProperties(IPropertyValueCollection properties);

        [AcceptVerbs("MKCOL")]
        HttpResponseMessage MakeCollection();

        [AcceptVerbs("COPY")]
        HttpResponseMessage Copy();

        [AcceptVerbs("MOVE")]
        HttpResponseMessage Move();

        [AcceptVerbs("LOCK")]
        HttpResponseMessage Lock();

        [AcceptVerbs("UNLOCK")]
        HttpResponseMessage Unlock();
    }
}
