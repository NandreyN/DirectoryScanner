using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    [Table("Addresses")]
    public class PoolItem
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public bool IsBusy { get; set; }
    }
}
