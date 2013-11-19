using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valley;
using Valley.Models;

namespace Valley.Storage
{
    public class LockManager : ResourceManager, ILockManager
    {
        private readonly object _token = new object();

        public LockManager(Uri baseUri)
            : base(baseUri)
        {
            Resources = new List<Uri>();
        }

        public IList<Uri> Resources { get; protected set; }

        public ILockToken CreateToken()
        {
            return new LockToken(BaseUri);
        }

        public void Save(ILockToken input)
        {
            lock (_token)
            {
                base.Save(input);
                if (!Resources.Contains(input.Resource))
                {
                    Resources.Add(input.Resource);
                }
            }
        }

        public new IResource Delete(Uri id)
        {
            lock (_token)
            {
                var item = base.Delete(id);
                var input = item as ILockToken;
                if (input != null)
                {
                    if (Resources.Contains(input.Resource))
                    {
                        Resources.Remove(input.Resource);
                    }
                }
                return item;
            }
        }

        public IList<IResource> DeleteAll(ILockToken proto)
        {
            lock (_token)
            {
                var temp = base.DeleteAll(proto);
                foreach (var item in temp)
                {
                    var input = item as ILockToken;
                    if (input != null)
                    {
                        if (!Resources.Contains(input.Resource))
                        {
                            Resources.Remove(input.Resource);
                        }
                    }
                }
                return temp;
            }
        }
    }
}
