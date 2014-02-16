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
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Valley
{
    public static class ConfigExtension
    {
        public static void Register(HttpConfiguration config)
        {
            var type = typeof(IConfigExtension);
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);            
            foreach(var c in types)
            {
                var method = c.GetMethod("Register");
                method.Invoke(null, new [] { config });            
            }
        }
    }
}
