using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Interfaces
{
    public interface IFolder
    {
        int Id { get;}
        string LocalName { get; }
        string AbsoluteName { get; }
    }
}
