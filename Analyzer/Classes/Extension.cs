using Newtonsoft.Json;
using ScannerService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scanner1Service.Classes
{
    public class Extension : IExtension
    {
        private const int EXAMPLES_COUNT = 2;

        [JsonProperty]
        public string Name { get; private set; }

        [JsonProperty]
        public int Count { get; private set; }

        [JsonProperty]
        public long SizeBytes { get; private set; }

        [JsonProperty]
        public IList<string> Examples { get; private set; }

        public Extension(string name, int count, long bytes, IList<string> examples)
        {
            if (!IsValid(name, count, bytes))
                throw new ArgumentException("Invalid arguments");

            Name = name;
            Count = count;
            SizeBytes = bytes;
            Examples = examples ?? new List<string>();
        }

        public Extension(string jsonView)
        {
            try
            {
                var ext = JsonConvert.DeserializeObject<Extension>(jsonView);
                if (!IsValid(ext.Name, ext.Count, ext.SizeBytes))
                    throw new ArgumentException("Invalid arguments");

                Name = ext.Name;
                Count = ext.Count;
                SizeBytes = ext.SizeBytes;
                Examples = ext.Examples;
            }
            catch (JsonSerializationException)
            { throw; }
        }

        public void Union(IExtension extension)
        {
            if (Name != extension.Name)
                throw new ArgumentException("Incompatible extension");
            if (!IsValid(extension))
                throw new ArgumentException("Parameter is not valid");


            try
            {
                int newCount = checked(Count + extension.Count);
                long newSize = checked(SizeBytes + extension.SizeBytes);

                Count = newCount;
                SizeBytes = newSize;

                if (Examples.Count <= EXAMPLES_COUNT && extension.Examples.Any())
                {
                    int i = 0;
                    while (Examples.Count <= EXAMPLES_COUNT && i < extension.Examples.Count)
                    {
                        Examples.Add(extension.Examples.ElementAt(i++));
                    }
                }
            }
            catch (OverflowException)
            { throw; }
        }

        public override string ToString() => JsonConvert.SerializeObject(this);

        private static bool IsValid(IExtension ex) => !string.IsNullOrEmpty(ex.Name) && ex.Count > 0 && ex.SizeBytes > 0;
        private bool IsValid(string name, int count, long bytes) =>
            !string.IsNullOrEmpty(name) && count > 0 && bytes > 0;
    }
}