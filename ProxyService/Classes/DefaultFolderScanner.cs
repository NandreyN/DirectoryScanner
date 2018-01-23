using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProxyService.Interfaces;

namespace ProxyService.Classes
{
    public abstract class DefaultFolderScanner : IFolderScanner
    {
        public IDictionary<int, IFolder> Folders { get; private set; }

        protected DefaultFolderScanner()
        {
            Folders = new Dictionary<int, IFolder>();
        }

        public abstract void CreateFolderStructure(IFolder folder);
    }
}
