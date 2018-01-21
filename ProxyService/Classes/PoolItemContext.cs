using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    public class PoolItemContext:DbContext
    {
        public PoolItemContext(DbContextOptions<PoolItemContext> options) : base(options) { }

        public DbSet<PoolItem> PoolItems { get; set; }

        
    }
}
