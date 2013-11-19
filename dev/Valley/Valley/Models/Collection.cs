using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Valley.Models
{
    [DataContract(Namespace = "urn:valley")]
    public class Collection : Resource, ICollection
    {
        public Collection()
            : base()
        {
            Resources = new List<IResource>();
        }
        public Collection(Uri[] mappings)            
            : base(mappings) {}
        public Collection(ICollection src)
            : base(src)
        {
            Resources = new List<IResource>(src.Resources);
        }
        [DataMember]
        public IList<IResource> Resources { get; protected set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var k = obj as Collection;
            if (k == null) return base.Equals(obj);

            return base.Equals(k)
                && (this.Resources == k.Resources);
        }

        //http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + base.GetHashCode();
                if (Resources != null)
                hash = hash * 23 + Resources.GetHashCode();
                return hash;
            }
        }
    }
}
