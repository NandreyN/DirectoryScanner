using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    public class QueryItemContext : DbContext
    {
        public DbSet<QueryItem> QueryItems { get; set; }

        public QueryItemContext(DbContextOptions<QueryItemContext> options)
            : base(options)
        {
        }
    }
}
