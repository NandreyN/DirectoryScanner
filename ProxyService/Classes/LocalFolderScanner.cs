using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using ProxyService.Interfaces;

namespace ProxyService.Classes
{
    public class LocalFolderScanner : DefaultFolderScanner
    {
        private Queue<IFolder> _queue;
        public LocalFolderScanner() : base() => _queue = new Queue<IFolder>();
        public LocalFolderScanner(IEnumerable<IFolder> collection) : base() => _queue = new Queue<IFolder>(collection);

        public override void CreateFolderStructure(IFolder folder)
        {
            _queue.Enqueue(folder);
            CreateFolderStructure();
        }

        public void CreateFolderStructure()
        {
            Folders.Clear();
            while (_queue.Count > 0)
            {
                IFolder currDir = _queue.Dequeue();
                Folders.Add(currDir.Id, currDir);
                foreach (var toInsert in currDir.SubDirsLocalFolder())
                    _queue.Enqueue(toInsert);
            }
        }

        public async Task<bool> WriteToTableAsync(string connectionString, string tableName)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqliteCommand command = connection.CreateCommand())
                {
                    SqliteTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        command.Transaction = transaction;
                        command.CommandText = $"INSERT INTO[{tableName}] (Folder) VALUES(@FolderTag)";
                        command.Parameters.AddWithValue("@FolderTag", "");
                        foreach (var folder in Folders.Values)
                            await WriteOneItemAsync(command, folder);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return true;
        }

        private async Task<int> WriteOneItemAsync(SqliteCommand command, IFolder folder)
        {
            command.Parameters["@FolderTag"].Value = folder.AbsoluteName;
            return await command.ExecuteNonQueryAsync();
        }
    }
}
