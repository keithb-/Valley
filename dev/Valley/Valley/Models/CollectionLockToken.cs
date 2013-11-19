using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Valley.Models
{
    [DataContract(Namespace = "urn:valley")]
    public class CollectionLockToken : LockToken, ICollectionLockToken
    {
        public CollectionLockToken(Uri baseUri, CollectionLockTokenDepth depth)
            : base(baseUri)
        {
            this.Depth = depth;
        }
        public CollectionLockToken(ILockToken src, CollectionLockTokenDepth depth)
            : base(src) 
        {
            this.Depth = depth;
        }
        [DataMember]
        public CollectionLockTokenDepth Depth { get; protected set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var k = obj as CollectionLockToken;
            if (k == null) return base.Equals(obj);

            return base.Equals(k)
                && (this.Depth == k.Depth);
        }

        //http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + base.GetHashCode();
                hash = hash * 23 + Depth.GetHashCode();
                return hash;
            }
        }
    }
}
