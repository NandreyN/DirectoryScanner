using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    public class FolderRecordContext : DbContext
    {
        public static string ConnectionString { get; set; }
        public DbSet<FolderRecord> Folders { get; set;}
        public FolderRecordContext(DbContextOptions<FolderRecordContext> options)
           : base(options)
        {}

        public FolderRecordContext() : base()
        { }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }
    }
}
