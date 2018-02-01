using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProxyService.Interfaces;
using ScannerService.Interfaces;

namespace Scanner1Service.Classes
{
    public static class FilesToExtensionsAdapter
    {
        public static ICollection<IExtension> FilesToExtensions(IEnumerable<IFile> files)
        {
            IDictionary<string,IExtension> extensions = new Dictionary<string, IExtension>();
            foreach (var file in files)
            {
                IExtension ext = new Extension(file.Extension, 1, file.SizeBytes,
                    new List<string>() {file.AbsoluteName});

                if (extensions.ContainsKey(file.Extension))
                {
                    extensions[file.Extension].Union(ext);
                }
                else
                {
                    extensions.Add(file.Extension,ext);
                }
            }

            return extensions.Values;
        }
    }
}
