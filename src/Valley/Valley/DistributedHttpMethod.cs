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
using System.Net.Http;

namespace Valley
{
    public class DistributedHttpMethod : HttpMethod
    {
        protected DistributedHttpMethod(string method) : base(method) { }
        public static readonly DistributedHttpMethod Unlock = new DistributedHttpMethod("UNLOCK");
        public static readonly DistributedHttpMethod Lock = new DistributedHttpMethod("LOCK");
        public static readonly DistributedHttpMethod Copy = new DistributedHttpMethod("COPY");
        public static readonly DistributedHttpMethod Move = new DistributedHttpMethod("MOVE");
        public static readonly DistributedHttpMethod PropertyPatch = new DistributedHttpMethod("PROPPATCH");
        public static readonly DistributedHttpMethod MakeCollection = new DistributedHttpMethod("MKCOL");
    }
}