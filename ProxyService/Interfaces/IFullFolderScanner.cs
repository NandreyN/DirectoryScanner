using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Interfaces
{
    public interface IFullFolderScanner : IFolderScanner
    {
        void CreateFullStructure(IFolder folder);
    }
}
