using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    [Table("TaskTable")]
    public class TaskItem
    {
        [Column("Id")]
        public int Id { get; set; }
        [Column("Token")]
        public string Token { get; set; }
        [Column("IsFinished")]
        public bool IsFinished { get; set; }
        [Column("IsSuccess")]
        public bool IsSuccess { get; set; }
    }
}
