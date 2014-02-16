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
    public class LockToken : Resource, ILockToken
    {
        public LockToken(Uri baseUri) 
            : base()
        {
            // RFC.4918 §6.5
            Value = "uuid" + Guid.NewGuid();
            Mappings.Add(new Uri(baseUri, Value.ToString()));
        }
        
        public LockToken(ILockToken src)
            : base(src)
        {
            this.Value = src.Value;
            this.Resource = src.Resource;
            this.Timeout = src.Timeout;
        }
        
        [DataMember]
        public string Value { get; protected set; }

        [DataMember]
        public Uri LockUri
        {
            get
            {
                if ((Mappings != null) && (1 <= Mappings.Count))
                {
                    return Mappings[0];
                }
                return null;
            }
        }

        [DataMember]
        public Uri Resource { get; set; }
        
        [DataMember]
        public TimeSpan Timeout { get; set; }
        
        public bool IsExpired()
        {
            return (DateTime.Now < this.CreationDate.Add(this.Timeout));
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var k = obj as LockToken;
            if (k == null) return base.Equals(obj);

            return base.Equals(k) 
                && (this.Value == k.Value) 
                && (this.Resource == k.Resource);
        }

        //http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + base.GetHashCode();
                if (Value != null)
                hash = hash * 23 + Value.GetHashCode();
                if (Resource != null)
                hash = hash * 23 + Resource.GetHashCode();
                return hash;
            }
        }
    }
}
