﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    public class PoolItemContext : DbContext
    {
        public DbSet<PoolItem> PoolItems { get; set; }

        public PoolItemContext(DbContextOptions<PoolItemContext> options)
            : base(options)
        {
        }
    }
}
