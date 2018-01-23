using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Interfaces
{
    public interface IFolderScanner
    {
        void CreateFolderStructure(IFolder folder);
    }
}
