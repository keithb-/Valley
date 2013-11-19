using System.Collections.Generic;

namespace Valley.Models
{
    public interface ICollection : IResource
    {
        // RFC.4918 §5.2
        IList<IResource> Resources { get; }
    }
}
