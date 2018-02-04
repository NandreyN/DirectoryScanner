using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using ProxyService.Interfaces;

namespace ProxyService.Classes
{
    /// <summary>
    /// This class is responsible for creating subfolder list and writing it to the external storage
    /// </summary>
    public class LocalFolderScanner : DefaultFolderScanner
    {
        private Queue<IFolder> _queue;
        public LocalFolderScanner() => _queue = new Queue<IFolder>();
        public LocalFolderScanner(IEnumerable<IFolder> collection) => _queue = new Queue<IFolder>(collection);

        public override void CreateFolderStructure(IFolder folder)
        {
            _queue.Enqueue(folder);
            CreateFolderStructure();
        }

        /// <summary>
        /// Bypasses subfolders using queue
        /// Pops directory , gets list of subdirectories and adds it to the queue
        /// Repeat until queue is empty
        /// </summary>
        public void CreateFolderStructure()
        {
            Folders.Clear();
            while (_queue.Count > 0)
            {
                LocalFolder currDir = (LocalFolder)_queue.Dequeue();
                Folders.Add(currDir.Id, currDir);
                foreach (var toInsert in currDir.SubDirsLocalFolder())
                    _queue.Enqueue(toInsert);
            }
        }

        public override IEnumerable<IFile> GetFiles(IFolder folder)
        {
            return ((LocalFolder)folder).EnumerateFiles();
        }

        /// <summary>
        /// Using EF
        /// </summary>
        /// <param name="context">FolderRecordContext for storing folder structure</param>
        /// <param name="token">Request identificator</param>
        /// <returns>true if succes , false in case of exception</returns>
        public async Task<bool> WriteToTableAsync(FolderRecordContext context, string token)
        {
            try
            {
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                for (int i = 0; i < Folders.Count; i++)
                {
                    if (i % 100 == 0)
                    {
                        await context.SaveChangesAsync();
                        context = new FolderRecordContext();
                        context.ChangeTracker.AutoDetectChangesEnabled = false;
                    }

                    IFolder fldr = Folders.Values.ElementAt(i);
                    context.Folders.Add(new FolderRecord() { Path = fldr.AbsoluteName, Token = token, WasDelivered = false,InnerToken = string.Empty});
                }
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                context.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }
    }
}
