using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScannerService.Interfaces
{
    public interface IExtension
    {
        [JsonProperty]
        string Name { get; }

        [JsonProperty]
        int Count { get; }

        [JsonProperty]
        long SizeBytes { get; }

        [JsonProperty]
        IList<string> Examples { get; }

        void Union(IExtension extension);
        string ToString();
    }
}
