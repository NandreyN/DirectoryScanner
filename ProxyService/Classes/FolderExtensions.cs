using ProxyService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    public static class FolderExtensions
    {
        public static IList<LocalFolder> SubDirsLocalFolder(this LocalFolder fldr)
        {
            DirectoryInfo di = new DirectoryInfo(fldr.AbsoluteName);
            if (!di.Exists)
                throw new IOException("Directory not exists");

            IList<LocalFolder> dirs = new List<LocalFolder>();
            foreach (var a in di.GetDirectories())
            {
                dirs.Add(new LocalFolder(a));
            }
            return dirs;
            //return Directory.GetDirectories(fldr.AbsoluteName, "*", SearchOption.AllDirectories).ToList<string>().ConvertAll(x => new LocalFolder(x));
        }

        public static ICollection<LocalFile> EnumerateFiles(this LocalFolder fldr)
        {
            DirectoryInfo di = new DirectoryInfo(fldr.AbsoluteName);
            if (!di.Exists)
                throw new IOException("Directory not exists");

            ICollection<LocalFile> files = new List<LocalFile>();
            foreach (var file in di.EnumerateFiles())
            {
                files.Add(new LocalFile(file));
            }
            return files;
        }
    }
}
