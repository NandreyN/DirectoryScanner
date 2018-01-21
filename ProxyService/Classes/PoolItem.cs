using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    [Table("Addresses")]
    public class PoolItem
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Address")]
        public string Address { get; set; }

        [Column("IsBusy")]
        public bool IsBusy { get; set; }
    }
}
