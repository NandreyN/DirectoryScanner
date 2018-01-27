using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Interfaces
{
    public enum PropertySelector { FolderStructureCreated }

    public interface IRecoveryManager
    {
        bool SetProperty(PropertySelector selector, string token, bool value);
        void Recover(string token);
    }
}
