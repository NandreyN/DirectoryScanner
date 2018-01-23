using ProxyService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    public static class Extensions
    {
        public static IList<IFolder> SubDirsLocalFolder(this IFolder fldr)
        {
            DirectoryInfo di = new DirectoryInfo(fldr.AbsoluteName);
            IList<IFolder> dirs = new List<IFolder>();
            foreach (var a in di.GetDirectories())
            {
                dirs.Add(new LocalFolder(a));
            }
            return dirs;
        }
    }
}
