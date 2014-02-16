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
using System.Text;

namespace Valley.Models
{
    [DataContract(Namespace = "urn:valley")]
    public class Resource : IResource
    {
        private List<Uri> _mappings;
        public Resource() 
        {
            Live = new PropertyValueCollection();
            Dead = new PropertyValueCollection();
            _mappings = new List<Uri>();
            CreationDate = DateTime.Now;
            UpdateDate = DateTime.Now;
        }
        public Resource(Uri[] mappings)
            : this()
        {
            _mappings.AddRange(mappings);
        }
        public Resource(IResource src) : this()
        {
            this.Mappings = src.Mappings;
            this.CreationDate = src.CreationDate;
            //TODO: Map the Live/Dead attributes.
        }
        [DataMember]
        public IList<Uri> Mappings { get { return _mappings; } set { _mappings = (List<Uri>)value; } }
        [DataMember]
        public IPropertyValueCollection Live { get; protected set; }
        [DataMember]
        public IPropertyValueCollection Dead { get; protected set; }
        [DataMember(Name = "creationdate")]
        public DateTime CreationDate { get; protected set; }
        [DataMember(Name = "displayname")]
        public string DisplayName { get; set; } 
        [DataMember(Name = "getcontentlanguage")]
        public string ContentLanguage { get; set; }
        [DataMember(Name = "getcontentlength")]
        public int ContentLength { get; set; }
        [DataMember(Name = "getcontenttype")]
        public string ContentType{ get; set; }
        [DataMember(Name = "getetag")]
        public string ETag { get; set; }
        [DataMember(Name = "getlastmodified")]
        public string LastModified { get; set; }
        [DataMember(Name = "lockdiscovery")]
        public int LockDiscovery { get; set; }
        [DataMember(Name = "resourcetype")]
        public int ResourceType { get; set; }
        [DataMember(Name = "supportedlock")]
        public int SupportedLock { get; set; }
        [DataMember]
        public DateTime UpdateDate { get; set; }

        private byte[] _content;
        public byte[] Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                ContentLength = _content.Length;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var k = obj as Resource;
            if (k == null) return base.Equals(obj);

            return (this.CreationDate == k.CreationDate)
                && (this.DisplayName == k.DisplayName)
                && (this.ContentLanguage == k.ContentLanguage)
                && (this.ContentLength == k.ContentLength)
                && (this.ContentType == k.ContentType)
                && (this.ResourceType == k.ResourceType);
        }

        //http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                if (CreationDate != null)
                hash = hash * 23 + CreationDate.GetHashCode();
                if (DisplayName != null)
                hash = hash * 23 + DisplayName.GetHashCode();
                if (ContentLanguage != null)
                hash = hash * 23 + ContentLanguage.GetHashCode();
                hash = hash * 23 + ContentLength.GetHashCode();
                hash = hash * 23 + ContentType.GetHashCode();
                hash = hash * 23 + ResourceType.GetHashCode();
                return hash;
            }
        }
    }

    public static class ResourceExtensions
    {
        public static IPropertyValueCollection GetStaticProperties(this IResource resource)
        {
            var result = new PropertyValueCollection();
            var prop = new Property();
            prop.Name = "creationdate";
            prop.PropertyType = typeof(DateTime);
            var value = new PropertyValue();
            value.Property = prop;
            value.Value = resource.CreationDate;
            result.Add(value);
            
            prop = new Property();
            prop.Name = "displayname";
            prop.PropertyType = typeof(string);
            value = new PropertyValue();
            value.Property = prop;
            value.Value = resource.DisplayName;
            result.Add(value);

            prop = new Property();
            prop.Name = "getcontentlanguage";
            prop.PropertyType = typeof(string);
            value = new PropertyValue();
            value.Property = prop;
            value.Value = resource.DisplayName;
            result.Add(value);

            prop = new Property();
            prop.Name = "getcontentlength";
            prop.PropertyType = typeof(int);
            value = new PropertyValue();
            value.Property = prop;
            value.Value = resource.ContentLength;
            result.Add(value);

            prop = new Property();
            prop.Name = "getcontenttype";
            prop.PropertyType = typeof(string);
            value = new PropertyValue();
            value.Property = prop;
            value.Value = resource.ContentType;
            result.Add(value);

            prop = new Property();
            prop.Name = "getetag";
            prop.PropertyType = typeof(string);
            value = new PropertyValue();
            value.Property = prop;
            value.Value = resource.ETag;
            result.Add(value);

            prop = new Property();
            prop.Name = "getlastmodified";
            prop.PropertyType = typeof(string);
            value = new PropertyValue();
            value.Property = prop;
            value.Value = resource.LastModified;
            result.Add(value);

            prop = new Property();
            prop.Name = "lockdiscovery";
            prop.PropertyType = typeof(int);
            value = new PropertyValue();
            value.Property = prop;
            value.Value = resource.LockDiscovery;
            result.Add(value);

            prop = new Property();
            prop.Name = "resourcetype";
            prop.PropertyType = typeof(int);
            value = new PropertyValue();
            value.Property = prop;
            value.Value = resource.ResourceType;
            result.Add(value);

            prop = new Property();
            prop.Name = "supportedlock";
            prop.PropertyType = typeof(int);
            value = new PropertyValue();
            value.Property = prop;
            value.Value = resource.SupportedLock;
            result.Add(value);

            prop = new Property();
            prop.Name = "lastmodified";
            prop.PropertyType = typeof(DateTime);
            value = new PropertyValue();
            value.Property = prop;
            value.Value = resource.UpdateDate;
            result.Add(value);

            return result;
        }
    }
}
