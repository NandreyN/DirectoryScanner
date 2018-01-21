using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Interfaces
{
    interface ITokenProvider
    {
        Task<(string, bool)> RegisterTokenAsync();
        Task<bool> UnregisterToken(string token); 
    }
}
