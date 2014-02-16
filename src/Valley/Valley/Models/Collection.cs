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
