using Newtonsoft.Json;
using ProxyService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    public class LocalFile : Interfaces.IFile
    {
        public LocalFile(string path) : this(new FileInfo(path))
        { }

        public LocalFile(FileInfo info)
        {
            FileAttributes attrib = File.GetAttributes(info.FullName);

            if (!File.Exists(info.FullName))
                throw new IOException("File not exists");
            if (!attrib.HasFlag(FileAttributes.Directory))
                throw new IOException("Not a file");

            LocalName = info.Name;
            AbsoluteName = info.FullName;
            Extension = info.Extension;
            SizeBytes = info.Length;
        }

        public string LocalName { get; private set; }

        public string AbsoluteName { get; private set; }

        public string Extension { get; private set; }

        public double SizeBytes { get; private set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
