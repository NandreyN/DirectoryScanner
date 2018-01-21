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

        public TaskItemContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>().ToTable("TaskTable");
        }
    }
}
