using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    public class TaskItemContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; }
        public static string ConnectionString { get; set; }

        public TaskItemContext(DbContextOptions<TaskItemContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }
    }
}
