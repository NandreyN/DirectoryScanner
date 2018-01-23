using ProxyService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    public class LocalFolder : IFolder
    {
        public LocalFolder(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
                throw new FileNotFoundException($"Folder {path} not found");

            Id = TotalCount++;
            LocalName = dirInfo.Name;
            AbsoluteName = dirInfo.FullName;
        }

        private static int TotalCount = 0;

        public int Id { get; private set; }

        public string LocalName { get; private set; }

        public string AbsoluteName { get; private set; }
    }
}
