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
        public int Id { get; set; }
        public string Token { get; set; }
        public bool IsFinished { get; set; }
        public bool IsSuccess { get; set; }
    }
}
