using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Valley.Models;

namespace Valley.Controllers
{
    public class LockController : ApiController
    {
        private readonly ILockManager _lockManager;

        public LockController(ILockManager lockManager)
        {
            _lockManager = lockManager;
        }

        public ICollection<IResource> Get()
        {
            return _lockManager.FindAll();
        }

        public IResource Get(string id)
        {
            return _lockManager.Find(Request.RequestUri);
        }

        [AcceptVerbs("PROPFIND")]
        public IPropertyValueCollection GetProperties()
        {
            throw new NotImplementedException();
        }
    }
}
