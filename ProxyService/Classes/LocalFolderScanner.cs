using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProxyService.Interfaces;

namespace ProxyService.Classes
{
    public class LocalFolderScanner : DefaultFolderScanner
    {
        private Queue<IFolder> _queue;

        public LocalFolderScanner() : base() => _queue = new Queue<IFolder>();

        public override void CreateFolderStructure(IFolder folder)
        {
            _queue.Enqueue(folder);
            while (_queue.Count > 0)
            {
                IFolder currDir = _queue.Dequeue();
                Folders.Add(currDir.Id, currDir);
                foreach (var toInsert in currDir.SubDirsLocalFolder())
                    _queue.Enqueue(toInsert);
            }
        }
    }
}
