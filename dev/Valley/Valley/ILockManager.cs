using System;
using System.Collections.Generic;
using Valley.Models;

namespace Valley
{
    public interface ILockManager : IResourceManager
    {
        IList<IResource> DeleteAll(ILockToken proto);
        IList<Uri> Resources { get; }
        void Save(ILockToken input);
        ILockToken CreateToken();
    }
}
