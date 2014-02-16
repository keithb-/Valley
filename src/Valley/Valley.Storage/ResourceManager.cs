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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using Valley;
using Valley.Models;

namespace Valley.Storage
{
    public class ResourceManager : IResourceManager
    {
        private readonly object _token = new object();
        private readonly Dictionary<Uri, IResource> _data;
        private readonly XmlDocument _masterOutline;
        private XPathNavigator _seek;
        private const string OutlineRoot = "hd0";

        public ResourceManager(Uri baseUri)
        {
            BaseUri = baseUri;
            _data = new Dictionary<Uri, IResource>();
            _masterOutline = new XmlDocument();
            _seek = _masterOutline.CreateNavigator();
            CreateDirectory(OutlineRoot);
            if ("/" != BaseUri.PathAndQuery)
            {
                var path = OutlineRoot;
                foreach (var s in BaseUri.Segments)
                {
                    path = path + s;
                    if ("/" == s) continue;
                    CreateDirectory(path);
                }
            }
        }

        public Uri BaseUri { get; protected set; }

        public int Count
        {
            get
            {
                return _data.Count;
            }
        }

        public bool Contains(Uri id)
        {
            var path = ToRelative(id);
            return _data.Keys.Contains(path);
        }

        public void Save(IResource input)
        {
            if (input.Mappings.Count == 0)
                throw new ArgumentException();

            lock (_token)
            {
                foreach (var k in input.Mappings)
                {
                    var path = ToRelative(k);
                    if (!_data.Keys.Contains(path))
                    {
                        CreateDirectory(path.ToString());
                        _data.Add(path, input);
                        continue;
                    }
                    _data[path] = input;
                }
            }
        }

        public IResource Delete(Uri id)
        {
            var path = ToRelative(id);
            if (_data.Keys.Contains(path))
            {
                lock (_token)
                {
                    Delete(path.ToString());
                    var temp = _data[path];
                    _data.Remove(path);
                    return temp;
                }
            }
            return default(IResource);
        }

        public IList<IResource> DeleteAll(IResource proto)
        {
            lock (_token)
            {
                var temp = _data.Where((key, value) => { return value.Equals(proto); });
                foreach (KeyValuePair<Uri, IResource> item in temp)
                {
                    Delete(item.Key.ToString());
                    _data.Remove(item.Key);
                }
                return temp.Select(item => item.Value).ToList();
            }
        }

        public IResource Find(Uri id)
        {
            IResource value;
            var path = ToRelative(id);
            _data.TryGetValue(path, out value);
            return value;
        }

        public IList<IResource> FindAll()
        {
            return _data.Values.ToList();
        }

        public IList<IResource> FindAll(IResource proto)
        {
            var temp = _data.Where((key, value) => { return value.Equals(proto); });
            foreach (KeyValuePair<Uri, IResource> item in temp)
            {
                _data.Remove(item.Key);
            }
            return temp.Select(item => item.Value).ToList();
        }

        //private void OutlineAdd(Uri input)
        //{
        //    var path = input.ToString();
        //    var start = path.LastIndexOf("/") + 1;
        //    var name = path.Substring(start);
        //    var directory = "";
        //    if (1 < start)
        //    {
        //        directory = path.Substring(0, start - 2);
        //    }
        //    var node = _masterOutline.SelectSingleNode(OutlineRoot + directory);
        //    var child = _masterOutline.CreateElement(name);
        //    node.AppendChild(child);
        //}

        //private void OutlineRemove(Uri input)
        //{
        //    var path = OutlineRoot + input;
        //    var node = _masterOutline.SelectSingleNode(path);
        //    node.RemoveChild(_masterOutline.SelectSingleNode(OutlineRoot + "/" + input.AbsoluteUri));
        //}

        //private bool DirectoryExists(Uri input)
        //{
        //    var path = OutlineRoot + input;
        //    return (_masterOutline.SelectSingleNode(path) != null);
        //}

        private Uri ToRelative(Uri input)
        {
            if (!input.IsAbsoluteUri)
            {
                return input;
            }
            if ((BaseUri.Host != input.Host) && (BaseUri.Port != input.Port))
            {
                throw new ArgumentOutOfRangeException("Host address mismatch.");
            }
            var path = input.AbsolutePath;
            if (path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length - 1);
            }
            return new Uri(OutlineRoot + path, UriKind.Relative);
        }

        public void CreateDirectory(string path)
        {
            if (path.StartsWith("/"))
            {
                path = path.Substring(1, path.Length - 1);
            }
            if (path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length - 1);
            }
            if (!path.Contains('/'))
            {
                lock (_token)
                {
                    _seek.MoveToRoot();
                    _seek.AppendChild("<" + path + "/>");
                }
                return;
            }

            var start = path.LastIndexOf('/');
            lock (_token)
            {
                var node = _seek.SelectSingleNode(path.Substring(0, start));
                if (node != null)
                {
                    node.AppendChild("<" + path.Substring(start + 1) + "/>");
                }
            }
        }

        public void CreateFile(string path)
        {
            CreateDirectory(path);
        }

        public void Delete(string path)
        {
            Delete(path, false);
        }

        public void Delete(string path, bool recursive)
        {
            if (!path.Contains('/'))
            {
                return; // Cannot delete root.
            }
            lock (_token)
            {
                var node = _masterOutline.SelectSingleNode(path);
                if ((node.HasChildNodes) && (!recursive))
                {
                    return; // Do not delete when child exists.
                }
                var start = path.LastIndexOf('/');
                var parent = _masterOutline.SelectSingleNode(path.Substring(0, start));
                parent.RemoveChild(node);
            }
        }

        public bool Exists(string path)
        {
            return (_masterOutline.SelectSingleNode(path) != null);
        }
    
    
    }
}
