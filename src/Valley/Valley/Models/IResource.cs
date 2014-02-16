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

namespace Valley.Models
{
    public interface IResource
    {
        // RFC.4918 §15.3
        string ContentLanguage { get; set; }
        string ContentType { get; set; }
        byte[] Content { get; set; }
        // RFC.4918 §15.4
        int ContentLength { get; }
        // RFC.4918 §15.1
        DateTime CreationDate { get; }
        // RFC.4918 §4.1, §4.3
        //kbielaczyc.2013.09.28: Properties do not have a Uri for key; they are assumed to be XML so they have 
        //a urn that is assumed to be unique across all namespaces and properties.
        IPropertyValueCollection Dead { get; }
        // RFC.4918 §15.2
        string DisplayName { get; set; }
        // RFC.4918 §4.1, §4.3
        IPropertyValueCollection Live { get; }
        // RFC.4918 §3
        IList<Uri> Mappings { get; set; }

        string ETag { get; set; }
        string LastModified { get; set; }
        int LockDiscovery { get; set; }
        int ResourceType { get; set; }
        int SupportedLock { get; set; }
        DateTime UpdateDate { get; }
    }
}
