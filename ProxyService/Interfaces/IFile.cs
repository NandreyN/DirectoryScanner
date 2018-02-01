using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Interfaces
{
    public interface IFile
    {
        string LocalName { get; }
        string AbsoluteName { get; }
        string Extension { get; }
        long  SizeBytes { get; }
    }
}
