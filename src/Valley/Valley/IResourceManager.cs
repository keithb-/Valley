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
using Valley.Models;

namespace Valley
{
    public interface IResourceManager
    {
        bool Contains(Uri id);
        void CreateDirectory(string path);
        void CreateFile(string path);
        void Delete(string path);
        void Delete(string path, bool recursive);
        IResource Delete(Uri id);
        IList<IResource> DeleteAll(IResource proto);
        bool Exists(string path);
        IResource Find(Uri id);
        IList<IResource> FindAll();
        IList<IResource> FindAll(IResource proto);
        void Save(IResource input);
    }
}
