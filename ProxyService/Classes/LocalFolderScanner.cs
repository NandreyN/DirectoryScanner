using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProxyService.Interfaces;

namespace ProxyService.Classes
{
    public class LocalFolderScanner : DefaultFolderScanner
    {
        public LocalFolderScanner() : base() { }

        public override void CreateFolderStructure(IFolder folder)
        {
            
        }
    }
}
