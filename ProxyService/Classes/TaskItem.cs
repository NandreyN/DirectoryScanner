using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    [Table("Tasks")]
    public class TaskItem
    {
        [Key]
        [Column("Token")]
        public string Token { get; set; }
        [Column("IsReady")]
        public bool IsReady { get; set; }
        [Column("Status")]
        public int Status { get; set; }
        [Column("WasStopped")]
        public bool WasStopped { get; set; }
    }
}
