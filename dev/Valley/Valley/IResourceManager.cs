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
